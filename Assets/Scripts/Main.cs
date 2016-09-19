using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    public PlayerScript player;
    public CameraScript cameraScript;
    public LightScript lightScript;
    public WallSpawnerScript spawner;
    public TextScriptBase gameText;
    public TextScriptBase[] menuText;
    public TextScriptBase[] uiText;
    public Counter counter;

    public static StateMachine s;

	void Start () 
    {
        s = new StateMachine();
        StartCoroutine(Starting());
	}

	void Update () 
    {
        if (s.StateChanged())
        {
            switch (s.CurrentState)
            {
                case State.Menu:
                    StartCoroutine(Menu());
                    break;

                case State.Playing:
                    StartCoroutine(Play());
                    break;

                case State.Stopping:
                    StartCoroutine(Stop());
                    break;

                case State.terminated:
                    Application.Quit();
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            s.MoveNext(Command.Stop);
	}

    private IEnumerator Starting()
    {
        Cursor.visible = false;
        cameraScript.ToggleMovement(true);

        yield return new WaitForSeconds(1);
        gameText.ChangeColor(Color.white, 0.02f);

        yield return new WaitForSeconds(2);
        lightScript.ChangeIntensity(0.8f, 0.02f);
        gameText.ChangeColor(new Color(1, 1, 1, 0), 0.02f);

        yield return new WaitForSeconds(1);
        Destroy(gameText.gameObject);
        player.ToggleMovement(true);
        s.MoveNext(Command.Continue);

    }

    private IEnumerator Menu()
    {
        foreach (TextScriptBase text in menuText)
            text.ChangeColor(Color.white, 0.03f);

        foreach (TextScriptBase text in uiText)
            text.ChangeColor(Color.white, 0.03f);


        player.ToggleSelecting(true);
        player.SelectTriangle();

        while (true)
        {
            int selectedTriangle = player.currentTriangle - cameraScript.ParseRotation();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (selectedTriangle < 0)
                    selectedTriangle += 6;

                switch (selectedTriangle)
                {
                    case 5: //upper triangle
                        s.MoveNext(Command.Play);
                        break;

                    case 2: //bottom triangle
                        s.MoveNext(Command.Stop);
                        break;
                }
            }

            if (s.CurrentState == State.Menu)
                yield return null;
            else
                break;
        }
    }

    private IEnumerator Play()
    {
        player.ToggleSelecting(false);

        counter.ResetCounter();
        counter.StartCounter();

        foreach (TextScriptBase text in menuText)
            text.ChangeColor(new Color(1, 1, 1, 0), 0.03f);

        spawner.StartSpawning();

        cameraScript.StartRotating();
        cameraScript.SetRotSpeed(2);

        yield return null;
    }

    private IEnumerator Stop()
    {
        WallScript.stopDelegate();

        counter.StopCounter();
        spawner.StopSpawning();

        while (cameraScript.StopRotating())
            yield return null;

        menuText[0].transform.parent.transform.eulerAngles = new Vector3(0, cameraScript.ParseRotation() * 60, 0);

        yield return new WaitForSeconds(1.5f);
        s.MoveNext(Command.Continue);
    }
}
