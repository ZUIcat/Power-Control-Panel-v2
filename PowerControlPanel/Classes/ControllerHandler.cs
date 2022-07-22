﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpDX.XInput;

namespace Power_Control_Panel.PowerControlPanel.Classes
{
    public class ControllerHandler
    {
        public Thread controllerThread;
        public Controller controller;
        private Gamepad gamepadCurrent;
        private  Gamepad gamepadOld;
        private int threadSleep;
        private bool readEvents = true;

        public buttonEvents events = new buttonEvents();

        public void createGamePadStateCollectorLoop(int threadSleepValue, bool readEventArg)
        {
            //controllerThread = new Thread(new ThreadStart(gamePadStateCollector));
            //controllerThread.IsBackground = true;
            threadSleep = threadSleepValue;
            readEvents = readEventArg;
            gamePadStateCollector();
        }

        public void gamePadStateCollector()
        {

            while (GlobalVariables.useRoutineThread)
            {

                if (controller == null)
                {
                    while (controller == null && GlobalVariables.useRoutineThread)
                    {
                        controller = new Controller(UserIndex.One);
                        if (controller == null) { Task.Delay(4000); }
                    }

                }
                if (controller != null)
                {
                    try
                    {
                        if (controller.IsConnected)
                        {
                            gamepadCurrent = controller.GetState().Gamepad;
                            Thread.Sleep(15);
                            while (GlobalVariables.useRoutineThread && controller.IsConnected)
                            {

                                gamepadOld = gamepadCurrent;
                                gamepadCurrent = controller.GetState().Gamepad;

                                if (!gamepadOld.Buttons.HasFlag(GamepadButtonFlags.A) && gamepadCurrent.Buttons.HasFlag(GamepadButtonFlags.A))
                                {
                                    events.RaiseEventA();

                                }
                                if (!gamepadOld.Buttons.HasFlag(GamepadButtonFlags.B) && gamepadCurrent.Buttons.HasFlag(GamepadButtonFlags.B))
                                {
                                    events.RaiseEventB();
                                }
                                if (!gamepadOld.Buttons.HasFlag(GamepadButtonFlags.X) && gamepadCurrent.Buttons.HasFlag(GamepadButtonFlags.X))
                                {
                                    events.RaiseEventX();
                                }
                                if (!gamepadOld.Buttons.HasFlag(GamepadButtonFlags.Y) && gamepadCurrent.Buttons.HasFlag(GamepadButtonFlags.Y))
                                {
                                    events.RaiseEventY();
                                }
                                if (!gamepadOld.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder) && gamepadCurrent.Buttons.HasFlag(GamepadButtonFlags.LeftShoulder))
                                {
                                    events.RaiseEventLB();

                                }
                                if (!gamepadOld.Buttons.HasFlag(GamepadButtonFlags.RightShoulder) && gamepadCurrent.Buttons.HasFlag(GamepadButtonFlags.RightShoulder))
                                {
                                    events.RaiseEventRB();

                                }
                                if (!gamepadOld.Buttons.HasFlag(GamepadButtonFlags.RightShoulder) && gamepadCurrent.Buttons.HasFlag(GamepadButtonFlags.RightShoulder))
                                {
                                    events.RaiseEventRB();

                                }
                                if (gamepadOld.RightTrigger == 0 && gamepadCurrent.RightTrigger > 0)
                                {
                                    events.RaiseEventRT();

                                }
                                if (gamepadOld.LeftTrigger == 0 && gamepadCurrent.LeftTrigger > 0)
                                {
                                    events.RaiseEventLT();

                                }

                                Thread.Sleep(threadSleep);
                            }


                        }
                        else
                        {
                            while (!controller.IsConnected && GlobalVariables.useRoutineThread)
                            {
                                Task.Delay(3000);
                                controller = new Controller(UserIndex.One);
                            }

                        }

                    }
                    catch
                    {

                    }



                }


            }


        }


    }

    public class buttonEvents 
    {
        public void RaiseEventRT()
        {
            pressRTEvent?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler pressRTEvent;
        public void RaiseEventLT()
        {
            pressLTEvent?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler pressLTEvent;

        public void RaiseEventA()
        {
            pressAEvent?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler pressAEvent;

        public event EventHandler pressXEvent;
        public void RaiseEventX()
        {
            pressXEvent?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler pressYEvent;
        public void RaiseEventY()
        {
            pressYEvent?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler pressBEvent;
        public void RaiseEventB()
        {
            pressBEvent?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler pressLBEvent;
        public void RaiseEventLB()
        {
            pressLBEvent?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler pressRBEvent;
        public void RaiseEventRB()
        {
            pressRBEvent?.Invoke(this, EventArgs.Empty);
        }
    }

}