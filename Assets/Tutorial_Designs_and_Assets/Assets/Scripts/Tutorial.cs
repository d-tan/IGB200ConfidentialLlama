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

    IngredientID[] cheesePizza = new IngredientID[] { IngredientID.Sauce, IngredientID.Cheese };

    public int tutorialProgression = 1;
    public int voteToProgress = 0;
    public int waitersSpawned = 0;
    public int ordersSpawned = 0;
    public int tutorialTestOrdersCompleted = 0;
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
    WaiterManager waiters;
	GameManager gameManager;
    public Plate plate;

    // Use this for initialization
    void Start () {
        orders = GameManager.GetComponent<OrderManager>();
        p1Receiver = P1OrderReciever.GetComponent<OrderReceiver>();
        p2Receiver = P2OrderReciever.GetComponent<OrderReceiver>();
        waiters = GameManager.GetComponent<WaiterManager>();
        gameManager = GetComponent<GameManager> ();

        if (completedTutorial == false) {
            //Set the level of tutorial progression to the very beginning
            tutorialProgression = 0;
            //tutorialProgression = 16; //Debug Move
			gameManager.TutorialBegin ();
            Activate();

        } else {
            //Set the level of tutorial progression to the end to skip the tutorial
            tutorialProgression = 16;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (tutorialProgression == 0) {
            player01Text.text = "Would you two like me to teach you the ropes of Pizza Flick? Or would you rather go straight to the action?";
            player02Text.text = player01Text.text;

            P1EndOptionYText.text = "Teach me! (Tutorial)";
            P2EndOptionYText.text = P1EndOptionYText.text;
            P1EndOptionNText.text = "We're good. (Skip Tutorial)";
            P2EndOptionNText.text = P1EndOptionNText.text;
        } else if (tutorialProgression == 1) {
            player01Text.text = "Welcome young chef to 'Flick Dish Pizzaria', the most lively pizzaria in the world.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Cool.";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 2){
            player01Text.text = "So the first step is to make food, you two will be working together on this. Ah, here comes an orders!";
            player02Text.text = player01Text.text;
            while (ordersSpawned != 1) {
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
        } else if (tutorialProgression == 4) {
            player01Text.text = "You can flick the ingredients around to share them between players. Every pizza needs a type of sauce, for these, use Pizza Sauce, drag it onto a pizza base to combine then.";
            player02Text.text = player01Text.text;

            if (plate.CheckContainsIngredient(IngredientID.Sauce)) {
                progressTutorial();
            }

            player01ResponseText.text = "Gotcha!";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 5) {
            player01Text.text = "For the cheese pizza, you will need to add, you guessed it; some cheese. if you don't have the cheese, get your friend to flick some to you.";
            player02Text.text = player01Text.text;

            if (plate.CheckContainsIngredient(IngredientID.Sauce) && plate.CheckContainsIngredient(IngredientID.Cheese)) {
                progressTutorial();
            }

            P1Response.SetActive(false);
            P2Response.SetActive(false);
        } else if (tutorialProgression == 6) {
            player01Text.text = "Nice. Unforunately we can't just send the pizzas off, people who eat cold pizzas are just weird. Flick or drag the pizza into the side of the oven.";
            player02Text.text = player01Text.text;

            if (tutorialOrdersCompleted == 1) {
                progressTutorial();
            }

            P1Response.SetActive(false);
            P2Response.SetActive(false);
        } else if (tutorialProgression == 7) {
            player01Text.text = "Now the pizza is cooked. If you had other orders you could be doing them right now. But for now, we wait.";
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
            player01Text.text = "Pizza Boys and Girls, we don't discriminate between gender here. We'll get through this quickly because you're probably sick of listening to me.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "What!? Not at all...";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 11) {
            player01Text.text = "Right... So these are our pizza boys and girls, they work here. They'll deliver the boxed pizza for you. They don't stop moving, we've given them roller scates to get around quicker.";
            player02Text.text = player01Text.text;

            waiters.tutorialSpawn = true;

            player01ResponseText.text = "Cool, and I get paid now right?";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 12) {
            player01Text.text = "Yep. Last thing though, try not to hit people with the food, it's a waste and you'll have to make it again... It's also rude.";
            player02Text.text = player01Text.text;

            player01ResponseText.text = "Ok.";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 13) {
            player01Text.text = "I'm going to place two orders for Cheese Pizzas and see if you can complete them yourselves. Good luck!";
            player02Text.text = player01Text.text;

            while (ordersSpawned < 4) {
                orders.CreateOrder(cheesePizza);
                orders.CreateOrder(cheesePizza);
                ordersSpawned += 2;
            }
            if (Time.timeSinceLevelLoad > timeBeforeBoxHide) {
                P1Tutorial.SetActive(false);
                P2Tutorial.SetActive(false);
            }
            if (tutorialTestOrdersCompleted == 2) {
                progressTutorial();
            }

            P1Response.SetActive(false);
            P2Response.SetActive(false);
        } else if (tutorialProgression == 14) {
            player01Text.text = "Very well done, you should feel proud of yourselves. The food is excellent.";
            player02Text.text = player01Text.text;

            P1Tutorial.SetActive(true);
            P2Tutorial.SetActive(true);

            P1Response.SetActive(true);
            P2Response.SetActive(true);

            player01ResponseText.text = "Thanks!";
            player02ResponseText.text = player01ResponseText.text;
        } else if (tutorialProgression == 15) {
            player01Text.text = "I think you're ready to go for real. What do you two say? You can say no and we'll do it again.";
            player02Text.text = player01Text.text;         

            P1EndOptionYText.text = "I'm ready!!";
            P2EndOptionYText.text = P1EndOptionYText.text;
            P1EndOptionNText.text = "Can you tell me again?";
            P2EndOptionNText.text = P1EndOptionNText.text;
        } else if (tutorialProgression == 16) {
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
			gameManager.TutorialEnded ();
            tutorialProgression++;
        }
        if (tutorialProgression >= 11 && tutorialProgression <= 16) {
            while (waitersSpawned < 4) {
                waiters.SpawnWaiter();
                waitersSpawned += 1;
            }
        }
    }

    public void progressTutorial() {
        if (tutorialProgression == 13) {
            timeBeforeBoxHide = Time.timeSinceLevelLoad + 10.0f;
        }

        if (tutorialProgression == 14) {
            Activate();
        }

        if (tutorialProgression == 15) {
            Activate();

        }

        if (tutorialProgression != 16) {
            tutorialProgression++;
            Activate();
            
        }
    }

    public void playerVote() {
        voteToProgress++;

        if (voteToProgress == 2) {
            progressTutorial();
            voteToProgress = 0;
            print("All Players Voted - Progressing");

            P1Response.SetActive(true);
            P2Response.SetActive(true);
        }

        if (voteToProgress < -1) {
            voteToProgress = 0;
            print("Player Voted to Redo - Resetting");

            P1Response.SetActive(true);
            P2Response.SetActive(true);

            P1EndOptionY.SetActive(false);
            P1EndOptionN.SetActive(false);
            P2EndOptionY.SetActive(false);
            P2EndOptionN.SetActive(false);

            waiters.tutorialSpawn = false;
            waitersSpawned = 0;
            ordersSpawned = 0;
            tutorialTestOrdersCompleted = 0;
            tutorialOrdersCompleted = 0;
            timeBeforeBoxHide = 0;

            tutorialProgression = 1;
        }

        if (voteToProgress > 150) {
            voteToProgress = 0;
            print("Players Voted to Skip - Skipping");

            P1Response.SetActive(false);
            P2Response.SetActive(false);

            P1EndOptionY.SetActive(false);
            P1EndOptionN.SetActive(false);
            P2EndOptionY.SetActive(false);
            P2EndOptionN.SetActive(false);

            waiters.tutorialSpawn = false;
            waitersSpawned = 0;
            ordersSpawned = 0;
            tutorialTestOrdersCompleted = 0;
            tutorialOrdersCompleted = 0;
            timeBeforeBoxHide = 0;
            completedTutorial = true;

            tutorialProgression = 16;
        }

        if (tutorialProgression == 0 && voteToProgress > 101) {
            progressTutorial();
            voteToProgress = 0;
            print("Players Voted differently - Progressing");

            P1Response.SetActive(true);
            P2Response.SetActive(true);
        }
    }

    public void Activate() {
        if (tutorialProgression == 0 || tutorialProgression == 15) {
            if (P1Response.activeInHierarchy == true || P2Response.activeInHierarchy == true) {
                P1Response.SetActive(false);
                P2Response.SetActive(false);
            }

            if (P1EndOptionY.activeInHierarchy == false || P1EndOptionN.activeInHierarchy == false) {
                P1EndOptionY.SetActive(true);
                P1EndOptionN.SetActive(true);
            }

            if (P2EndOptionY.activeInHierarchy == false || P2EndOptionN.activeInHierarchy == false) {
                P2EndOptionY.SetActive(true);
                P2EndOptionN.SetActive(true);
            }
        }
        if (tutorialProgression > 0 && tutorialProgression < 15) {
            if (P1Response.activeInHierarchy == false || P2Response.activeInHierarchy == false) {
                P1Response.SetActive(true);
                P2Response.SetActive(true);
            }

            if (P1EndOptionY.activeInHierarchy == true || P1EndOptionN.activeInHierarchy == true) {
                P1EndOptionY.SetActive(false);
                P1EndOptionN.SetActive(false);
            }

            if (P2EndOptionY.activeInHierarchy == true || P2EndOptionN.activeInHierarchy == true) {
                P2EndOptionY.SetActive(false);
                P2EndOptionN.SetActive(false);
            }
        }
    }
}
