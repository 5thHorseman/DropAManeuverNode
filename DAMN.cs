// DROP A MANEUVER NODE 0.1
// By 5thHorseman
// License: CC SA
// v0.1: Added a toolbar button to drop a maneuver node 5 minutes in the future.
// Most of the code for the toolbar, and the logic for showing and hiding the gui is from KSPCasher.

using KSP.UI.Screens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DropAManeuverNode
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class DropAManeuverNode : MonoBehaviour
    {
        ApplicationLauncherButton ToolbarButton;
        private bool ShowGUI = false;

        public void Awake()
        {
            GameEvents.onGUIApplicationLauncherReady.Add(OnGUIApplicationLauncherReady);
        }

        public ManeuverNode DropAManeuverNode_Time(double seconds)
        {
            return FlightGlobals.ActiveVessel.patchedConicSolver.AddManeuverNode(Planetarium.GetUniversalTime() + (seconds));
        }

        public ManeuverNode DropAManeuverNode_UT(double seconds)
        {
            return FlightGlobals.ActiveVessel.patchedConicSolver.AddManeuverNode(seconds);
        }

        private void OnGUIApplicationLauncherReady()
        {
            if (ToolbarButton == null)
            {
                ToolbarButton = ApplicationLauncher.Instance.AddModApplication(GUIOn, GUIOff, null, null, null, null, ApplicationLauncher.AppScenes.MAPVIEW, GameDatabase.Instance.GetTexture("DropAManeuverNode/DAMN_Toolbar", false));
            }
        }

        public void GUIOff()
        {
            ShowGUI = false;
        }

        public void GUIOn()
        {
            ShowGUI = true;
        }

        private void OnGUI()
        {
            if (ShowGUI)
            {
                MainMenu = GUI.Window(0, MainMenu, DrawMainMenu, "Drop A Maneuver Node");
            }
        }

        public Rect MainMenu = new Rect(20, 20, 235, 200);
        void DrawMainMenu(int windowID)
        {
            ManeuverNode node;
            double time;
            int vpos = 20;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            if (GUI.Button(new Rect(5, vpos, 200, 20), "5 Minutes From Now"))
            {
                DropAManeuverNode_Time(5 * 60);
            }
            if (GUI.Button(new Rect(5, vpos += 25, 200, 20), "At Periapsis"))
            {
                time = FlightGlobals.ActiveVessel.orbit.timeToPe;
                if (time == 0)
                {
                    node = DropAManeuverNode_Time(time);
                    // x is radial
                    // y is normal
                    // z is grade
                    // node.DeltaV.z = 1000;
                }
            }
            if (GUI.Button(new Rect(5, vpos += 25, 200, 20), "At Apoapsis"))
            {
                time = FlightGlobals.ActiveVessel.orbit.timeToAp;
                if (time == 0)
                {
                    node = DropAManeuverNode_Time(time);
                }
            }
            if (GUI.Button(new Rect(5, vpos += 25, 200, 20), "Halfway Around Orbit"))
            {
                time = FlightGlobals.ActiveVessel.orbit.period/2;
                if (time == 0)
                {
                    node = DropAManeuverNode_Time(time);
                }
            }
            if (GUI.Button(new Rect(5, vpos += 25, 200, 20), "Next Oribt"))
            {
                time = FlightGlobals.ActiveVessel.orbit.period;
                if (time == 0)
                {
                    node = DropAManeuverNode_Time(time);
                }
            }
            if (GUI.Button(new Rect(5, vpos += 25, 200, 20), "Next Closest Approach"))
            {
                time = FlightGlobals.ActiveVessel.orbit.closestTgtApprUT;
                if (time == 0)
                {
                    node = DropAManeuverNode_UT(time);
                }
            }
            if (GUI.Button(new Rect(5, vpos += 25, 200, 20), "timeToTransition1"))
            {
                time = FlightGlobals.ActiveVessel.orbit.timeToTransition1;
                if (time == 0)
                {
                    node = DropAManeuverNode_Time(time);
                }
            }
            if (GUI.Button(new Rect(5, vpos += 25, 200, 20), "timeToTransition2"))
            {
                time = FlightGlobals.ActiveVessel.orbit.timeToTransition2;
                if (time == 0)
                {
                    node = DropAManeuverNode_Time(time);
                }
            }
        }
    }
}