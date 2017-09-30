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
    public GameObject DraggablePlate;

    IngredientID[] cheesePizza = new IngredientID[] { IngredientID.PizzaBase, IngredientID.Cheese };

    public int tutorialProgression = 1;
    public int voteToProgress = 0;
    public int ordersSpawned = 0;

    public Text player01Text;
    public Text player02Text;

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
        //plate = DraggablePlate.GetComponent<Plate>();



        if (completedTutorial == false) {
            //Set the level of tutorial progression to the very beginning
            tutorialProgression = 1;
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
            while (ordersSpawned != 1) {
                orders.CreateOrder(cheesePizza);
                ordersSpawned++;
            }

            player01ResponseText.text = "Good timing.";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 3) {
            player01Text.text = "Ok, so one of you drag the order to your side of the parlour to accept it. Pressing on the order once you have it lets you see what you need for it.";
            player02Text.text = player01Text.text;
            if (p1Receiver.currentOrder != null || p2Receiver.currentOrder != null) {
                progressTutorial();
            }

            P1Response.SetActive(false);
            P2Response.SetActive(false);
            //player01ResponseText.text = "<TRIGGER PROGRESS>";
            //player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 4) {
            player01Text.text = "It doesn't matter what order you make the pizza in, we'll get the idea. Drag the pizza base to a plate, if you don't have it, ask your friend to flick it to you.";
            player02Text.text = player01Text.text;

            if (plate.CheckContainsIngredient(IngredientID.PizzaBase)) {
                progressTutorial();
            }

            P1Response.SetActive(false);
            P2Response.SetActive(false);
            //player01ResponseText.text = "<TRIGGER PROGRESS>";
            //player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 5) {
            player01Text.text = "Next put some cheese on the pizza, again if you don't have the cheese, get your friend to flick some to you.";
            player02Text.text = player01Text.text;

            if (plate.CheckContainsIngredient(IngredientID.PizzaBase) && plate.CheckContainsIngredient(IngredientID.Cheese)) {
                progressTutorial();
            }

            P1Response.SetActive(false);
            P2Response.SetActive(false);
            //player01ResponseText.text = "<TRIGGER PROGRESS>";
            //player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 6) {
            player01Text.text = "Nice, one pizza. Unforunately we can't just send that off, pizzas need to be cooked, people who eat cold pizzas are just weird. Flick the pizza into the oven.";
            player02Text.text = player01Text.text;

            P1Response.SetActive(false);
            P2Response.SetActive(false);
            //player01ResponseText.text = "<TRIGGER PROGRESS>";
            //player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 7) {
            player01Text.text = "So, now the pizza goes into the oven and is cooked. If you had other orders you could be doing them right now. But for now, we wait.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Ok.";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 8) {
            player01Text.text = "It's random what side the pizza comes out on, but that shouldn't be a problem for you.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Nope.";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 9) {
            player01Text.text = "Delicious! Now that it's hot, it's ready to serve.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Yay!";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 10) {
            player01Text.text = "Ok, time to alert you to a norm around here, obstructions. We'll get through these quickly because you're probably sick of listening to me.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "What!? Not at all...";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 11) {
            player01Text.text = "Right... So these are our pizza boys and girls, they work here. Soon enough they'll pick up the pizza and take it away. But sadly we aren't done yet.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "We aren't?";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 12) {
            player01Text.text = "Nope. Obstructions! I sabotaged two of your appliances. Your fridge is broken and the oven is on fire. No need to thank me.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "...";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 13) {
            player01Text.text = "For fires, drag the fire extinguisher onto the appliance to put it out. Don't worry, your food is safe. The appliance will just start again.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Gotcha!";
            player02ResponseText.text = player02ResponseText.text;
        } else if (tutorialProgression == 14) {
            player01Text.text = "For broken appliances, tap on it three times to fix it.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Right!";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 15) {
            player01Text.text = "Better get to it!";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "<TRIGGER PROGRESS>";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 16) {
            player01Text.text = "Well done, and that's it. Last thing, try not to hit the pizza boys and girls with the pizza, it's a waste of pizza. AND it's rude, it's rude.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Ok.";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 17) {
            player01Text.text = "I'm going to place two orders for Cheese Pizzas and see if you can complete them yourselves. Good luck!";
            player02Text.text = player01Text.text;

            while (ordersSpawned < 2) {
                orders.CreateOrder(cheesePizza);
                orders.CreateOrder(cheesePizza);
                ordersSpawned += 2;
            }

            player01ResponseText.text = "Let's do this!";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 18) {
            player01Text.text = "Very well done, your progress, not the food. The food is excellent.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Thanks!";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 19) {
            player01Text.text = "I think you're ready to go for real. What do you two say? You can say no and we'll do it again.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "I'm ready!!";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 20) {
            player01Text.enabled = false;
            player02Text.enabled = false;
            player01ResponseText.enabled = false;
            player02ResponseText.enabled = false;
            completedTutorial = true;
        } else if (tutorialProgression < 1 || tutorialProgression > 20){
            print("Tutorial Error: tutorialProgression is not >= 1 and <= 20.");
        }
    }

    public void progressTutorial() {
        if (tutorialProgression != 20) {
            tutorialProgression++;
        }
    }

    public void playerVote() {
        voteToProgress++;

        if (gameObject.tag == "Player01Tutorial") {
            P1Response.SetActive(false);
        }
        if (gameObject.tag == "Player02Tutorial") {
            P2Response.SetActive(false);
        }
        if (voteToProgress == 2) {
            progressTutorial();
            voteToProgress = 0;
            print("All Players Voted - Progressing");
        }
    }
}
