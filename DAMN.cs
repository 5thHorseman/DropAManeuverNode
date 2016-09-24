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
        private bool ShowedGUI = false;

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
                ToolbarButton = ApplicationLauncher.Instance.AddModApplication(ToggleGUI, ToggleGUI, null, null, null, null, ApplicationLauncher.AppScenes.MAPVIEW, GameDatabase.Instance.GetTexture("DropAManeuverNode/DAMN_Toolbar", false));
            }
        }

        public double timeToTargetAN(Orbit orbit)
        {
            Orbit orbitT = FlightGlobals.fetch.VesselTarget.GetOrbit();
            Vector3d nodeAsc = Vector3d.Cross(orbitT.GetOrbitNormal(), orbit.GetOrbitNormal());
            double anomalyAsc = orbit.GetTrueAnomalyOfZupVector(nodeAsc) * Mathf.Rad2Deg;
            double time = orbit.GetDTforTrueAnomaly(anomalyAsc * Mathf.Deg2Rad, orbit.period);
            return time < 0.0 ? time + orbit.period : time;
        }

        public double timeToTargetDN(Orbit orbit)
        {
            Orbit orbitT = FlightGlobals.fetch.VesselTarget.GetOrbit();
            Vector3d nodeAsc = Vector3d.Cross(orbit.GetOrbitNormal(), orbitT.GetOrbitNormal());
            double anomalyAsc = orbit.GetTrueAnomalyOfZupVector(nodeAsc) * Mathf.Rad2Deg;
            double time = orbit.GetDTforTrueAnomaly(anomalyAsc * Mathf.Deg2Rad, orbit.period);
            return time < 0.0 ? time + orbit.period : time;
        }

        public double timeToAN(Orbit orbit)
        {
            double time = orbit.GetDTforTrueAnomaly((360.0 - orbit.argumentOfPeriapsis) * UnityEngine.Mathf.Deg2Rad, orbit.period);
            return time < 0.0 ? time + orbit.period : time;
        }

        public double timeToDN(Orbit orbit)
        {
            double time = orbit.GetDTforTrueAnomaly((180.0 - orbit.argumentOfPeriapsis) * UnityEngine.Mathf.Deg2Rad, orbit.period);
            return time < 0.0 ? time + orbit.period : time;
        }

        public void ToggleGUI()
        {
            ShowGUI = !ShowGUI;
            if (!ShowGUI) ShowedGUI = false;
        }

        private void OnGUI()
        {
            if (ShowGUI)
            {
                MainMenu = GUI.Window(0, MainMenu, DrawMainMenu, "Drop A Maneuver Node");
                if (!ShowedGUI)
                {
                    MainMenu.x = Mouse.screenPos.x - 210;
                    MainMenu.y = Mouse.screenPos.y + 5;
                    ShowedGUI = true;
                }
            }
        }

        public Rect MainMenu = new Rect(200, 200, 210, 20 + (9 * 25) + 5);

        void DrawMainMenu(int windowID)
        {
            ManeuverNode node;
            double time;
            int vpos = 0;
            int hpos = 5;
            GUI.DragWindow(new Rect(0, 0, 10000, 20));

            // ======================================================= TIME ===============================================
            GUI.Label(new Rect(hpos, vpos += 25, 200, 20), "Time:");
            if (GUI.Button(new Rect(hpos, vpos += 25, 35, 20), "1m"))
            {
                DropAManeuverNode_Time(60);
                ShowGUI = false;
            }
            if (GUI.Button(new Rect(hpos += 40, vpos, 40, 20), "5m"))
            {
                DropAManeuverNode_Time(5 * 60);
                ShowGUI = false;
            }
            if (GUI.Button(new Rect(hpos += 40, vpos, 40, 20), "10m"))
            {
                DropAManeuverNode_Time(10 * 60);
                ShowGUI = false;
            }
            if (GUI.Button(new Rect(hpos += 40, vpos, 40, 20), "1h"))
            {
                DropAManeuverNode_Time(60 * 60);
                ShowGUI = false;
            }
            if (GUI.Button(new Rect(hpos += 40, vpos, 40, 20), "1d"))
            {
                DropAManeuverNode_Time(6 * 60 * 60);
                ShowGUI = false;
            }
            hpos = 5;
            // ======================================================= ABSOLUTE ===============================================
            GUI.Label(new Rect(hpos, vpos += 25, 200, 20), "Orbit:");
            if (GUI.Button(new Rect(hpos, vpos += 25, 45, 20), "Pe"))
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
            if (GUI.Button(new Rect(hpos += 50, vpos, 45, 20), "Ap"))
            {
                time = FlightGlobals.ActiveVessel.orbit.timeToAp;
                if (time != 0)
                {
                    node = DropAManeuverNode_Time(time);
                    ShowGUI = false;
                }
            }
            if (GUI.Button(new Rect(hpos += 50, vpos, 45, 20), "AN"))
            {
                time = timeToAN(FlightGlobals.ActiveVessel.orbit);
                if (time != 0)
                {
                    node = DropAManeuverNode_Time(time);
                    ShowGUI = false;
                }
            }
            if (GUI.Button(new Rect(hpos += 50, vpos, 45, 20), "DN"))
            {
                time = timeToDN(FlightGlobals.ActiveVessel.orbit);
                if (time != 0)
                {
                    node = DropAManeuverNode_Time(time);
                    ShowGUI = false;
                }
            }
            hpos = 5;
            if (GUI.Button(new Rect(hpos, vpos += 25, 95, 20), "1/2 Orbit"))
            {
                time = FlightGlobals.ActiveVessel.orbit.period / 2;
                if (time != 0)
                {
                    node = DropAManeuverNode_Time(time);
                    ShowGUI = false;
                }
            }
            if (GUI.Button(new Rect(hpos += 100, vpos, 95, 20), "1 Orbit"))
            {
                time = FlightGlobals.ActiveVessel.orbit.period;
                if (time != 0)
                {
                    node = DropAManeuverNode_Time(time);
                    ShowGUI = false;
                }
            }
            hpos = 5;
            // ======================================================= TARGET ===============================================
            GUI.Label(new Rect(hpos, vpos += 25, 200, 20), "Target:");
            if (GUI.Button(new Rect(hpos, vpos += 25, 95, 20), "AN"))
            {
                time = timeToTargetAN(FlightGlobals.ActiveVessel.orbit);
                if (time != 0)
                {
                    node = DropAManeuverNode_Time(time);
                    ShowGUI = false;
                }
            }
            if (GUI.Button(new Rect(hpos += 100, vpos, 95, 20), "DN"))
            {
                time = timeToTargetDN(FlightGlobals.ActiveVessel.orbit);
                if (time != 0)
                {
                    node = DropAManeuverNode_Time(time);
                    ShowGUI = false;
                }
            }
            MainMenu.height = vpos + 25;

            //The below don't work so I'm removing them until I figure out why.
            /*            if (GUI.Button(new Rect(5, vpos += 25, 200, 20), "Next Closest Approach"))
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
            */
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