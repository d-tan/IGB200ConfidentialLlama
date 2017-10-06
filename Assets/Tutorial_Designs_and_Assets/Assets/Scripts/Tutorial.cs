using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public GameObject GameManager;

    public GameObject P1Response;
    public GameObject P2Response;

    public GameObject P1OrderReciever;
    public GameObject P2OrderReciever;

    public GameObject P1EndOptionY;
    public GameObject P2EndOptionY;
    public GameObject P1EndOptionN;
    public GameObject P2EndOptionN;

    public GameObject P1Tutorial;
    public GameObject P2Tutorial;

    public GameObject DraggablePlate;

    IngredientID[] cheesePizza = new IngredientID[] { IngredientID.PizzaBase, IngredientID.Cheese };

    public int tutorialProgression = 1;
    public int voteToProgress = 0;
    public int ordersSpawned = 0;
    public int tutorialOrdersCompleted = 0;
    public float timeBeforeBoxHide = 0;

    public Text player01Text;
    public Text player02Text;

    public Text P1EndOptionYText;
    public Text P2EndOptionYText;
    public Text P1EndOptionNText;
    public Text P2EndOptionNText;

    public Text player01ResponseText;
    public Text player02ResponseText;

    public bool completedTutorial = false;

    OrderManager orders;
    OrderReceiver p1Receiver;
    OrderReceiver p2Receiver;
    public Plate plate;

    // Use this for initialization
    void Start () {
        orders = GameManager.GetComponent<OrderManager>();
        p1Receiver = P1OrderReciever.GetComponent<OrderReceiver>();
        p2Receiver = P2OrderReciever.GetComponent<OrderReceiver>();

        if (completedTutorial == false) {
            //Set the level of tutorial progression to the very beginning
            tutorialProgression = 1;
            tutorialProgression = 16; //Debug Move
        } else {
            //Set the level of tutorial progression to the end to skip the tutorial
            tutorialProgression = 19;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (tutorialProgression == 1) {
            player01Text.text = "Welcome young chef to 'Flick Dish Pizzaria', the most lively pizzaria in the world.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Cool.";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 2){
            player01Text.text = "So the first step is to make food. Ah, here comes an order!";
            player02Text.text = player01Text.text;
            while (ordersSpawned != 2) {
                orders.CreateOrder(cheesePizza);
                ordersSpawned++;
            }

            player01ResponseText.text = "Good timing.";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 3) {
            player01Text.text = "Ok, so drag one of the orders to your side of the parlour to accept it. Tap on the order to see what you need for it.";
            player02Text.text = player01Text.text;
            if (p1Receiver.currentOrder != null || p2Receiver.currentOrder != null) {
                progressTutorial();
            }

            P1Response.SetActive(false);
            P2Response.SetActive(false);
            //player01ResponseText.text = "<TRIGGER PROGRESS>";
            //player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 4) {
            /*player01Text.text = "It doesn't matter what order you make the pizza in, we'll get the idea. Drag the pizza base to a plate, if you don't have it, ask your friend to flick it to you.";
            player02Text.text = player01Text.text;

            if (plate.CheckContainsIngredient(IngredientID.PizzaBase)) {
                progressTutorial();
            }

            P1Response.SetActive(false);
            P2Response.SetActive(false);
            //player01ResponseText.text = "<TRIGGER PROGRESS>";
            //player02ResponseText.text = player01ResponseText.text;*/

            player01Text.text = "You can flick the ingredients around to move them. Every pizza starts with the base, that's what you put ingredients on.";
            player02Text.text = player01Text.text;

            if (plate.CheckContainsIngredient(IngredientID.PizzaBase)) {
                progressTutorial();
            }

            //P1Response.SetActive(false);
            //P2Response.SetActive(false);
            player01ResponseText.text = "Gotcha!";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 5) {
            player01Text.text = "For the cheese pizza, you will need to add, you guessed it; some cheese. if you don't have the cheese, get your friend to flick some to you.";
            player02Text.text = player01Text.text;

            if (plate.CheckContainsIngredient(IngredientID.PizzaBase) && plate.CheckContainsIngredient(IngredientID.Cheese)) {
                progressTutorial();
            }

            P1Response.SetActive(false);
            P2Response.SetActive(false);
            //player01ResponseText.text = "<TRIGGER PROGRESS>";
            //player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 6) {
            player01Text.text = "Nice, one pizza. Unforunately we can't just send that off, people who eat cold pizzas are just weird. Flick the pizza into the side of the oven.";
            player02Text.text = player01Text.text;

            P1Response.SetActive(false);
            P2Response.SetActive(false);
            //player01ResponseText.text = "<TRIGGER PROGRESS>";
            //player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 7) {
            player01Text.text = "Now the pizza goes into the oven and is cooked. If you had other orders you could be doing them right now. But for now, we wait.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Ok.";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 8) {
            player01Text.text = "We have a state of the art oven, the 'Super Pizza Cooker 5000', it even boxes the pizzas!";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Wow, fancy. Especially for a place like this.";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 9) {
            player01Text.text = "So when the pizza comes out, it should be ready to g... Wait, what did you say?";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Nothing! What's next?";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 10) {
            player01Text.text = "Obstructions! Every workplace has problems. We'll get through these quickly because you're probably sick of listening to me.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "What!? Not at all...";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 11) {
            player01Text.text = "Right... So these are our pizza boys and girls, they work here. They'll deliver the boxed pizza for you.";
            player02Text.text = player01Text.text;

            //START THE WAITERS WALKING

            player01ResponseText.text = "And I get paid now right?";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 12) {
            player01Text.text = "You get points, yes. Next, gotta teach you what to do in case of a fire, how to put it out!";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Can't we call the fire department?";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 13) {
            player01Text.text = "No, draws too much attention. For fires, drag the fire extinguisher to the fire to put it out.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Gotcha!";
            player02ResponseText.text = player02ResponseText.text;
        } else if (tutorialProgression == 14) {
            player01Text.text = "Don't worry about your food too, it doesn't burn the pizza strangely...";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Ok... Cool!";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 15) {
            player01Text.text = "Better get to it!";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "<TRIGGER PROGRESS>";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 16) {
            player01Text.text = "And that's it. Last thing, try not to hit people with the food, it's a waste and you'll have to make it again... It's also rude.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Ok.";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 17) {
            player01Text.text = "I'm going to place two orders for Cheese Pizzas and see if you can complete them yourselves. Good luck!";
            player02Text.text = player01Text.text;

            Debug.Log (Time.timeSinceLevelLoad);

            while (ordersSpawned < 2) {
                orders.CreateOrder(cheesePizza);
                orders.CreateOrder(cheesePizza);
                ordersSpawned += 2;
            }
            if (Time.timeSinceLevelLoad > timeBeforeBoxHide) {
                P1Tutorial.SetActive(false);
                P2Tutorial.SetActive(false);
            }

            P1Response.SetActive(false);
            P2Response.SetActive(false);
            //player01ResponseText.text = "Let's do this!";
            //player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 18) {
            player01Text.text = "Very well done, you should feel proud of yourselves. The food is excellent.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Thanks!";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 19) {
            player01Text.text = "I think you're ready to go for real. What do you two say? You can say no and we'll do it again.";
            player02Text.text = player01Text.text;

            P1Response.SetActive(false);
            P2Response.SetActive(false);

            P1EndOptionYText.text = "I'm ready!!";
            P2EndOptionYText.text = P1EndOptionYText.text;
            P1EndOptionNText.text = "Can you tell me again?";
            P2EndOptionNText.text = P1EndOptionNText.text;
        } else if (tutorialProgression == 20) {
            P1Tutorial.SetActive(false);
            P2Tutorial.SetActive(false);

            P1Response.SetActive(false);
            P2Response.SetActive(false);

            player01ResponseText.enabled = false;
            player02ResponseText.enabled = false;

            P1EndOptionY.SetActive(false);
            P1EndOptionN.SetActive(false);
            P2EndOptionY.SetActive(false);
            P2EndOptionN.SetActive(false);

            completedTutorial = true;
            tutorialProgression++;
        } else if (tutorialProgression < 1 || tutorialProgression > 20){
            print("Tutorial Error: tutorialProgression is not >= 1 and <= 21.");
        }
    }

    public void progressTutorial() {
        if (tutorialProgression == 16) {
            timeBeforeBoxHide = Time.timeSinceLevelLoad + 10.0f;
        }
        if (tutorialProgression == 17) {
            P1Tutorial.SetActive(true);
            P2Tutorial.SetActive(true);
        }
        if (tutorialProgression == 18) {
            P1EndOptionY.SetActive(true);
            P1EndOptionN.SetActive(true);
            P2EndOptionY.SetActive(true);
            P2EndOptionN.SetActive(true);
        }
        if (tutorialProgression != 20) {
            tutorialProgression++;
            
        }
    }

    public void playerVote() {
        voteToProgress++;

        //this.gameObject.SetActive(false);

        /*if (this.gameObject.tag == "Player02Tutorial") {
            P2Response.SetActive(false);
            print("This object is part of the tutorial02");
        }*/

        if (voteToProgress == 2) {
            progressTutorial();
            voteToProgress = 0;
            print("All Players Voted - Progressing");

            P1Response.SetActive(true);
            P2Response.SetActive(true);
        }

        if (voteToProgress < 0) {
            voteToProgress = 0;
            print("Player Voted to Redo - Resetting");

            P1Response.SetActive(true);
            P2Response.SetActive(true);

            P1EndOptionY.SetActive(false);
            P1EndOptionN.SetActive(false);
            P2EndOptionY.SetActive(false);
            P2EndOptionN.SetActive(false);

            tutorialProgression = 1;
        }
    }
}
