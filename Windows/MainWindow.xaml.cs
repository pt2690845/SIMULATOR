using SIMULATOR.Classes;
using SIMULATOR.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SIMULATOR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        Timer variableIndicators;
        Timer stateIndicators;
        public MainWindow()
        {
            InitializeComponent();
            ConfigComponents();

            // Iniciamos las tareas de comprobación de variables en segundo plano
            MultiThreadOperation.thread.Start();

            // Configuramos el estado inicial de los botones
            DisableButton(ref StopComs);
            DisableButton(ref StopSimulation);

            // Configuramos el estado inicial del banner de estados
            StatusSimulationDisconnected();
            StatusComsDisconnected();
        }
        private void ConfigComponents()
        {
            // Config the main motor
            mainMotor.RotationSpeed = 10;

            // Create links between the main motor movement and the cabin
            mainCabin.TranslationSpeed = mainMotor.LinearSpeed;
            mainMotor.MotorStartClockwise += mainCabin.StartTranslateDown;
            mainMotor.MotorStartCounterClockwise += mainCabin.StartTranslateUp;
            mainMotor.MotorStop += mainCabin.StopTranslate;
                
            // Create links between the main motor movement and the counterWeight
            mainCounterWeight.TranslationSpeed = mainMotor.LinearSpeed;
            mainMotor.MotorStartClockwise += mainCounterWeight.StartTranslateUp;
            mainMotor.MotorStartCounterClockwise += mainCounterWeight.StartTranslateDown;
            mainMotor.MotorStop += mainCounterWeight.StopTranslate;

            // Config the door motor
            doorMotor.RotationSpeed = 1;

            // Create the links between the door motor and the right door
            rightDoor.TranslationSpeed = doorMotor.LinearSpeed;
            doorMotor.MotorStartClockwise += rightDoor.StartOpen;
            doorMotor.MotorStartCounterClockwise += rightDoor.StartClose;
            doorMotor.MotorStop += rightDoor.Stop;

            // Create the links between the door motor and the left door
            leftDoor.TranslationSpeed = doorMotor.LinearSpeed;
            doorMotor.MotorStartClockwise += leftDoor.StartOpen;
            doorMotor.MotorStartCounterClockwise += leftDoor.StartClose;
            doorMotor.MotorStop += leftDoor.Stop;

            // Config the floor switch limits
            const int cabinSize = 145, delay = 50;
            firstFloorLimit.ObjectToTrack = mainCabin;
            firstFloorLimit.PositionToCheckOn = Canvas.GetTop(firstFloorLimit);
            firstFloorLimit.PositionToCheckOff = Canvas.GetTop(firstFloorLimit) + cabinSize;
            MultiThreadOperation.threadFunctions += firstFloorLimit.PositionCheckingGreaterOrEqual;

            secondFloorLimit.ObjectToTrack = mainCabin;
            secondFloorLimit.PositionToCheckOn = Canvas.GetTop(secondFloorLimit);
            firstFloorLimit.PositionToCheckOff = Canvas.GetTop(secondFloorLimit) + cabinSize;
            MultiThreadOperation.threadFunctions += secondFloorLimit.PositionCheckingGreaterOrEqual;

            thirdFloorLimit.ObjectToTrack = mainCabin;
            thirdFloorLimit.PositionToCheckOn = Canvas.GetTop(thirdFloorLimit);
            thirdFloorLimit.PositionToCheckOff = Canvas.GetTop(thirdFloorLimit) + cabinSize;
            MultiThreadOperation.threadFunctions += thirdFloorLimit.PositionCheckingGreaterOrEqual;

            fourthFloorLimit.ObjectToTrack = mainCabin;
            fourthFloorLimit.PositionToCheckOn = Canvas.GetTop(fourthFloorLimit);
            fourthFloorLimit.PositionToCheckOff = Canvas.GetTop(fourthFloorLimit) + cabinSize;
            MultiThreadOperation.threadFunctions += fourthFloorLimit.PositionCheckingGreaterOrEqual;

            // Config the door swith limits
            doorOpenLimit.Visibility = Visibility.Collapsed;
            doorOpenLimit.ObjectToTrack = rightDoor;
            doorOpenLimit.PositionToCheckOn = rightDoor.OpenPosition;
            MultiThreadOperation.threadFunctions += doorOpenLimit.PositionCheckingEqual;

            doorClosedLimit.Visibility = Visibility.Collapsed;
            doorClosedLimit.ObjectToTrack = rightDoor;
            doorClosedLimit.PositionToCheckOn = rightDoor.ClosedPositionDoor1;
            MultiThreadOperation.threadFunctions += doorClosedLimit.PositionCheckingEqual;

            // Create the link between the cabin and the objects inside it
            mainCabin.ObjectsInside.Add(rightDoor);
            mainCabin.ObjectsInside.Add(leftDoor);
            mainCabin.ObjectsInside.Add(doorOpenLimit);
            mainCabin.ObjectsInside.Add(doorClosedLimit);
            mainCabin.ObjectsInside.Add(mainCabinLoadIndicator);

            // Config the sliderWithLimit
            mainCabinLoadIndicator.sliderToTrack = myCabinSensors.loadSensor;
            myCabinSensors.loadSensor.objectsToUpdate += mainCabinLoadIndicator.UpdateValue;
            // Establishing a limit value changes the behaviour of the basic slider to a slider with limit

            // Config the state of the operator buttons
            MultiThreadOperation.threadFunctions += UpdateOperationMode;
        }
        private void UpdatePLCInputsOnSimulator()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                // Cabin buttons
                PLCMemory.Inputs[0].Value = firstFloorButtonCabin.Pressed;
                PLCMemory.Inputs[1].Value = secondFloorButtonCabin.Pressed;
                PLCMemory.Inputs[2].Value = thirdFloorButtonCabin.Pressed;
                PLCMemory.Inputs[3].Value = fourthFloorButtonCabin.Pressed;
                PLCMemory.Inputs[4].Value = closeDoorButtonCabin.Pressed;
                PLCMemory.Inputs[5].Value = openDoorButtonCabin.Pressed;

                // Floor buttons
                PLCMemory.Inputs[6].Value = firstFloorButtonHall.Pressed;
                PLCMemory.Inputs[7].Value = secondFloorButtonHall.Pressed;
                PLCMemory.Inputs[8].Value = thirdFloorButtonHall.Pressed;
                PLCMemory.Inputs[9].Value = fourthFloorButtonHall.Pressed;

                // Limit switches
                PLCMemory.Inputs[10].Value = firstFloorLimit.Status;
                PLCMemory.Inputs[11].Value = secondFloorLimit.Status;
                PLCMemory.Inputs[12].Value = thirdFloorLimit.Status;
                PLCMemory.Inputs[13].Value = fourthFloorLimit.Status;
                PLCMemory.Inputs[14].Value = doorOpenLimit.Status;
                PLCMemory.Inputs[15].Value = doorClosedLimit.Status;

                // Cabin sensors
                PLCMemory.Inputs[17].Value = myCabinSensors.loadSensor.IsAboveLimit;
                PLCMemory.Inputs[16].Value = myCabinSensors.doorSensor.Toggled;

                // Application inputs
                PLCMemory.Inputs[18].Value = systemActivation.Toggled;
                PLCMemory.Inputs[19].Value = !systemActivation.Toggled;
                PLCMemory.Inputs[20].Value = operationMode.Toggled;
                PLCMemory.Inputs[21].Value = !operationMode.Toggled;
                PLCMemory.Inputs[22].Value = stopEmergency.Toggled;
                PLCMemory.Inputs[23].Value = !stopEmergency.Toggled;
                PLCMemory.Inputs[24].Value = firstFloorEmergency.Toggled;
                PLCMemory.Inputs[25].Value = !firstFloorEmergency.Toggled;
                PLCMemory.Inputs[26].Value = goUp.Pressed;
                PLCMemory.Inputs[27].Value = goDown.Pressed;
                PLCMemory.Inputs[28].Value = ignore.Pressed;
            }));
        }
        private void updatePLCOutputsOnSimulator()
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                // Cabin LED indicatos
                firstFloorButtonCabin.CallIndicator = PLCMemory.Outputs[0].Value;
                secondFloorButtonCabin.CallIndicator = PLCMemory.Outputs[1].Value;
                thirdFloorButtonCabin.CallIndicator = PLCMemory.Outputs[2].Value;
                fourthFloorButtonCabin.CallIndicator = PLCMemory.Outputs[3].Value;

                // Floor LED indicatos
                firstFloorButtonHall.CallIndicator = PLCMemory.Outputs[4].Value;
                secondFloorButtonHall.CallIndicator = PLCMemory.Outputs[5].Value;
                thirdFloorButtonHall.CallIndicator = PLCMemory.Outputs[6].Value;
                fourthFloorButtonHall.CallIndicator = PLCMemory.Outputs[7].Value;

                // Motors
                mainMotor.MotorLeft = PLCMemory.Outputs[8].Value;
                mainMotor.MotorRight = PLCMemory.Outputs[9].Value;
                doorMotor.MotorLeft = PLCMemory.Outputs[11].Value;
                doorMotor.MotorRight = PLCMemory.Outputs[10].Value;

                // Operator LED indicators
                goUp.CallIndicator = PLCMemory.Outputs[12].Value;
                goDown.CallIndicator = PLCMemory.Outputs[13].Value;
            }));
        }
        private void StopSimulation_Click(object sender, RoutedEventArgs e)
        {
            // Stop Executing simulation tasks
            MultiThreadOperation.doTask = false;

            // Destroy indicatos
            StopUpdatingVariableIndicators();
            StopUpdatingStateIndicators();
            EliminateVariableIndicators();
            EliminateStateIndicators();

            // Visuals
            DisableButton(ref StopSimulation);
            EnableButton(ref StartSimulation);
            StatusSimulationDisconnected();
        }
        private void StartSimulation_Click(object sender, RoutedEventArgs e)
        {
            // Execute simulation tasks
            MultiThreadOperation.doTask = true;

            // Read all file variables
            Config.ReadVariables();

            // Create and update  indicators
            CreateVariableIndicators();
            CreateStateIndicators();
            StartUpdatingVariableIndicators();
            StartUpdatingStateIndicatos();

            // Update inputs and outputs
            MultiThreadOperation.threadFunctions += UpdatePLCInputsOnSimulator;
            MultiThreadOperation.threadFunctions += updatePLCOutputsOnSimulator;

            //Visual
            DisableButton(ref StartSimulation);
            EnableButton(ref StopSimulation);
            StatusSimulationConnected();
        }
        private void StartComs_Click(object sender, RoutedEventArgs e)
        {
            PLCMemory.Connect();
            if (PLCMemory.ConnectionStatus)
            {
                PLCMemory.StartAutoUpdating(TimeSpan.FromMilliseconds(100));

            //Visual
            DisableButton(ref StartComs);
            EnableButton(ref StopComs);
            StatusComsConnected();
            }
        }
        private void StopComs_Click(object sender, RoutedEventArgs e)
        {
            PLCMemory.Disconnect();
            
            /// Visual
            if(!PLCMemory.ConnectionStatus)
            {
                DisableButton(ref StopComs);
                EnableButton(ref StartComs);
                StatusComsDisconnected();
            }
        }
        private void ComsData_Click(object sender, RoutedEventArgs e)
        {
            plcMemoryWindow myplcMemoryMindow = new plcMemoryWindow();
            myplcMemoryMindow.Show();
        }
        private void SimulatorTest_Click(object sender, RoutedEventArgs e)
        {
            simulatorTest mySimulatorTest = new simulatorTest();
            mySimulatorTest.Show();
        }
        private void Config_Click(object sender, RoutedEventArgs e)
        {
            configWindow myConfigWindow = new configWindow();
            myConfigWindow.Show();
        }
        private void DisableButton(ref Button mybutton)
        {
            mybutton.IsHitTestVisible = false;
            mybutton.BorderBrush = Brushes.Transparent;
        }
        private void EnableButton(ref Button mybutton)
        {
            mybutton.IsHitTestVisible = true;
            mybutton.BorderBrush = this.Resources["foregorundHighlight"] as SolidColorBrush;
        }
        private void UpdateOperationMode()
        {
            if(operationMode.Toggled == false)
            {
                DisableOperatorButtons();
            }
            else
            {
                EnableOperatorButtons();
            }
        }
        private void DisableOperatorButtons()
        {
            if (!operatorButtonContainer.Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(DispatcherPriority.Background, CambiaColor);
            }
            else
            {
                CambiaColor();
            }
        }
        private void CambiaColor()
        {
            operatorButtonContainer.BorderBrush = Brushes.Transparent;
        }
        private void EnableOperatorButtons()
        {
            if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                {
                    operatorButtonContainer.BorderBrush = this.Resources["foregorundHighlight"] as SolidColorBrush; ;
                    goUp.buttonLineIndicator.Stroke = this.Resources["foregorundHighlight"] as SolidColorBrush;
                    goDown.buttonLineIndicator.Stroke = this.Resources["foregorundHighlight"] as SolidColorBrush;
                    ignore.buttonLineIndicator.Stroke = this.Resources["foregorundHighlight"] as SolidColorBrush;
                    operatorButtonContainer.IsHitTestVisible = true;
                }));
            }
        }
        private void StatusComsConnected()
        {
            conectionState.Fill = Brushes.Green;
        }
        private void StatusComsDisconnected()
        {
            conectionState.Fill = Brushes.Red;
        }
        private void StatusSimulationConnected()
        {
            simulationState.Text = "en ejecución";
        }
        private void StatusSimulationDisconnected()
        {
            simulationState.Text = "desactivada";
        }
        private void CreateVariableIndicators()
        {
            for (int i = 0; i < PLCMemory.InternalVariables.Count(); i++)
            {
                boolIndicator myBoolIndicator = new boolIndicator();
                myBoolIndicator.VariableName = PLCMemory.InternalVariables[i].Name;
                VariableContainer.Children.Add(myBoolIndicator);
            }
        }
        private void EliminateVariableIndicators()
        {
            VariableContainer.Children.Clear();
        }
        private void UpdateVariableIndicators(Object? StateInfo)
        {
            for (int i = 0; i < PLCMemory.InternalVariables.Count(); i++)
            {
                if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
                {
                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        boolIndicator myBoolIndicator = VariableContainer.Children[i] as boolIndicator;
                        myBoolIndicator.State = PLCMemory.InternalVariables[i].Value;
                    }));
                }
            }
        }
        private void StartUpdatingVariableIndicators()
        {
            variableIndicators = new Timer(UpdateVariableIndicators, null, 0, 500);
        }
        private void StopUpdatingVariableIndicators()
        {
            variableIndicators.Change(Timeout.Infinite, Timeout.Infinite);
        }
        private void CreateStateIndicators()
        {
            bool[] alreadyChecked = new bool[PLCMemory.States.Count()];
            int count = 0;
            for (int i = 0; i < PLCMemory.States.Count(); i++)
            {
                if (PLCMemory.States[i].StateGroupName != null && alreadyChecked[i] == false)
                {
                    stateIndicator myStateIndicator = new stateIndicator();
                    myStateIndicator.StateGroupName = PLCMemory.States[i].StateGroupName;
                    List<State> groupState = new List<State> { };
                    groupState.Add(new State(PLCMemory.States[i].Name, PLCMemory.States[i].Value));
                    for (int j = i + 1; j < PLCMemory.States.Count; j++)
                    {
                        if(PLCMemory.States[j].StateGroupName == PLCMemory.States[i].StateGroupName)
                        {
                            groupState.Add(new State(PLCMemory.States[j].Name, PLCMemory.States[j].Value));
                            alreadyChecked[j] = true;
                        }
                    }
                    myStateIndicator.GroupStates = groupState;
                    if (count % 3 == 0) fistColumnStateIndicators.Children.Add(myStateIndicator);
                    else if (count % 3 == 1) secondColumnStateIndicators.Children.Add(myStateIndicator);
                    else thirdColumnStateIndicator.Children.Add(myStateIndicator);
                    count++;
                }
            }
        }
        private void UpdateStateIndicators(Object? StateInfo)
        {
            for (int i = 0; i < PLCMemory.States.Count(); i++)
            {
                if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
                {
                    Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        UpdateStateIndicatorInColumn(fistColumnStateIndicators, i);
                        UpdateStateIndicatorInColumn(secondColumnStateIndicators, i);
                        UpdateStateIndicatorInColumn(thirdColumnStateIndicator, i);
                    }));
                }
            }
        }
        private void UpdateStateIndicatorInColumn(StackPanel myStackPanel, int i)
        {
            foreach (stateIndicator myStateIndicator in myStackPanel.Children)
            {
                if (myStateIndicator.StateGroupName != PLCMemory.States[i].StateGroupName) continue;
                foreach (State myState in myStateIndicator.GroupStates)
                {
                    if (myState.Name == PLCMemory.States[i].Name)
                    {
                        myState.Value = PLCMemory.States[i].Value;
                        myStateIndicator.updateStateInfo();
                        break;
                    }
                }
                break;
            }
        }
        private void EliminateStateIndicators()
        {
            fistColumnStateIndicators.Children.Clear();
            secondColumnStateIndicators.Children.Clear();
            thirdColumnStateIndicator.Children.Clear();
        }
        private void StartUpdatingStateIndicatos()
        {
            stateIndicators = new Timer(UpdateStateIndicators, null, 0, 500);
        }
        private void StopUpdatingStateIndicators()
        {
            stateIndicators.Change(Timeout.Infinite, Timeout.Infinite);
        }
    }
}
