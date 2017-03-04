using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

namespace Assets.Scripts
{
    public class TestModalPanel : MonoBehaviour
    {

        public Sprite icon;
        public Transform spawnPoint;
        public GameObject thingToSpawn;
        // public DisplayManager displayManager;

        private ModalPanel modalPanel;
        // private UnityAction myYesAction;
        // private UnityAction myNoAction;
        // private UnityAction myCancelAction;

        void Awake()
        {
            modalPanel = ModalPanel.Instance();
            //  myYesAction = new UnityAction (TestYesFunction);
            //  myNoAction = new UnityAction (TestNoFunction);
            //  myCancelAction = new UnityAction(TestCancelFunction);
        }

        //  Send to the Modal Panel to set up the Buttons and functions to call
        //public void TestC()
        //{
        //    modalPanel.Choice("This is an announcement!\nIf you don't like it, shove off!", TestCancelFunction);
        //    //  modalPanel.Choice ("This is an announcement!\nIf you don't like it, shove off!", myCancelAction);
        //}
        //public void TestCI()
        //{
        //    modalPanel.Choice("This is an announcement!\nIf you don't like it, shove off!", icon, TestCancelFunction);
        //    //  modalPanel.Choice ("This is an announcement!\nIf you don't like it, shove off!", myCancelAction);
        //}

        //public void TestYN()
        //{
        //    modalPanel.Choice("Cheese on your burger?", TestYesFunction, TestNoFunction);
        //    //  modalPanel.Choice ("Cheese on your burger?", myYesAction, myNoAction);
        //}

        //public void TestYNC()
        //{
        //    modalPanel.Choice("Would you like a poke in the eye?\nHow about with a sharp stick?", TestYesFunction, TestNoFunction, TestCancelFunction);
        //    //  modalPanel.Choice ("Would you like a poke in the eye?\nHow about with a sharp stick?", myYesAction, myNoAction, myCancelAction);
        //}

        //public void TestYNI()
        //{
        //    modalPanel.Choice("Do you like this icon?", icon, TestYesFunction, TestNoFunction, TestCancelFunction);
        //    //  modalPanel.Choice ("Do you like this icon?", icon, myYesAction, myNoAction, myCancelAction);
        //}

        //public void TestYNCI()
        //{
        //    modalPanel.Choice("Do you want to use this icon?", icon, TestYesFunction, TestNoFunction, TestCancelFunction);
        //    //  modalPanel.Choice ("Do you like this icon?", icon, myYesAction, myNoAction, myCancelAction);
        //}

        //public void TestLambda()
        //{
        //    modalPanel.Choice("Do you want to create this sphere?", () => { InstantiateObject(thingToSpawn); }, TestNoFunction);
        //    //  modalPanel.Choice ("Do you want to create this sphere?", () => { InstantiateObject(thingToSpawn); }, myNoAction);
        //}

        //public void TestLambda2()
        //{
        //    modalPanel.Choice("Do you want to create two spheres?", () => { InstantiateObject(thingToSpawn, thingToSpawn); }, TestNoFunction);
        //    //  modalPanel.Choice ("Do you want to create two spheres?", () => { InstantiateObject(thingToSpawn, thingToSpawn); }, myNoAction);
        //}

        //public void TestLambda3()
        //{
        //    modalPanel.Choice("Do you want to create three spheres?", () => { InstantiateObject(thingToSpawn); InstantiateObject(thingToSpawn, thingToSpawn); }, TestNoFunction);
        //    //  modalPanel.Choice ("Do you want to create three spheres?", () => { InstantiateObject(thingToSpawn); InstantiateObject(thingToSpawn, thingToSpawn); }, myNoAction);
        //}

        //  The function to call when the button is clicked
        //  These are wrapped up in a UnityAction during Awake
        void TestYesFunction()
        {
            // displayManager.DisplayMessage("Heck, yeah!");
        }

        void TestNoFunction()
        {
            //displayManager.DisplayMessage("No way, Jose!");
        }

        void TestCancelFunction()
        {
            //displayManager.DisplayMessage("I give up!");
        }

        void InstantiateObject(GameObject thingToInstantiate)
        {
            //displayManager.DisplayMessage("Here you go!");
            Instantiate(thingToInstantiate, spawnPoint.position, spawnPoint.rotation);
        }

        void InstantiateObject(GameObject thingToInstantiate, GameObject thingToInstantiate2)
        {
            //displayManager.DisplayMessage("Here you go!");
            Instantiate(thingToInstantiate, spawnPoint.position - Vector3.one, spawnPoint.rotation);
            Instantiate(thingToInstantiate2, spawnPoint.position + Vector3.one, spawnPoint.rotation);
        }
    }
}