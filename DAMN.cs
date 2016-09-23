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

        public void OnDisable()
        {
            GameEvents.onGUIApplicationLauncherReady.Remove(OnGUIApplicationLauncherReady);
            ApplicationLauncher.Instance.RemoveModApplication(ToolbarButton);
            GameEvents.onGUIApplicationLauncherReady.Remove(OnGUIApplicationLauncherReady);
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
                ToolbarButton = ApplicationLauncher.Instance.AddModApplication(ToggleGUI, ToggleGUI, null, null, null, null, ApplicationLauncher.AppScenes.MAPVIEW | ApplicationLauncher.AppScenes.TRACKSTATION, GameDatabase.Instance.GetTexture("DropAManeuverNode/DAMN_Toolbar", false));
            }
        }

        public void ToggleGUI()
        {
            ShowGUI = !ShowGUI;
        }

        private void OnGUI()
        {
            if (ShowGUI)
            {
                MainMenu = GUI.Window(0, MainMenu, DrawMainMenu, "Drop A Maneuver Node");
            }
        }

        public Rect MainMenu = new Rect(200, 200, 205, 235);
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
                if (time != 0)
                {
                    node = DropAManeuverNode_Time(time);
                    // x is radial, y is normal, z is -grade
                    // node.DeltaV.z = 1000;
                    ShowGUI = false;
                }
            }
            if (GUI.Button(new Rect(5, vpos += 25, 200, 20), "At Apoapsis"))
            {
                time = FlightGlobals.ActiveVessel.orbit.timeToAp;
                if (time != 0)
                {
                    node = DropAManeuverNode_Time(time);
                    ShowGUI = false;
                }
            }
            if (GUI.Button(new Rect(5, vpos += 25, 200, 20), "Halfway Around Orbit"))
            {
                time = FlightGlobals.ActiveVessel.orbit.period/2;
                if (time != 0)
                {
                    node = DropAManeuverNode_Time(time);
                    ShowGUI = false;
                }
            }
            if (GUI.Button(new Rect(5, vpos += 25, 200, 20), "Next Oribt"))
            {
                time = FlightGlobals.ActiveVessel.orbit.period;
                if (time != 0)
                {
                    node = DropAManeuverNode_Time(time);
                    ShowGUI = false;
                }
            }
            if (GUI.Button(new Rect(5, vpos += 25, 200, 20), "Next Closest Approach"))
            {
                time = FlightGlobals.ActiveVessel.orbit.closestTgtApprUT;
                {
                    print("DAMN: closest approach time = " + time);
                    node = DropAManeuverNode_UT(time);
                    ShowGUI = false;
                }
            }
            if (GUI.Button(new Rect(5, vpos += 25, 200, 20), "timeToTransition1 (?)"))
            {
                time = FlightGlobals.ActiveVessel.orbit.timeToTransition1;
                print("DAMN: Transition1 Time = " + time);
                if (time != 0)
                {
                    node = DropAManeuverNode_Time(time);
                    ShowGUI = false;
                }
            }
/*            if (GUI.Button(new Rect(5, vpos += 25, 200, 20), "DEBUG"))
            {
                print("DAMN: START DEBUG HERE!!!!!!!!!");
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.GetTimeToPeriapsis());
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.altitude);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.argumentOfPeriapsis);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.closestTgtApprUT);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.eccentricAnomaly);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.eccentricity);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.EndUT);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.epoch);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.FEVp);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.fromE);
                print("DAMN: ------------------------------");
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.inclination);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.LAN);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.mag);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.meanAnomaly);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.meanAnomalyAtEpoch);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.meanMotion);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.ObT);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.ObTAtEpoch);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.orbitalEnergy);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.orbitalSpeed);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.orbitPercent);
                print("DAMN: ------------------------------");
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.period);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.radius);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.semiMajorAxis);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.StartUT);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.timeToAp);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.timeToPe);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.timeToTransition1);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.trueAnomaly);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.ClAppr);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.ClEctr1);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.ClEctr2);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.CrAppr);
                print("DAMN: ------------------------------");
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.E);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.FEVs);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.fromV);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.nearestTT);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.nextTT);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.sampleInterval);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.SEVp);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.SEVs);
                print("DAMN: ------------------------------");
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.timeToTransition2);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.toE);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.toV);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.UTappr);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.UTsoi);
                print("DAMN: " + FlightGlobals.ActiveVessel.orbit.V);
                print("DAMN: END DEBUG HERE!!!!!!!!!");
            } */
        }
    }
}