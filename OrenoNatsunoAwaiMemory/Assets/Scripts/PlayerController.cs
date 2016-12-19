using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    // public int interval;
    float lpy;
    private int flag = 1;
    private int sCounter = 3;

    public GameObject waterM;
    public GameObject bomb1;
    public GameObject bomb2;
    public GameObject bomb3;
    public GameObject kc;
    public GameObject meron;

    public int delay;
    public int interval;

    // Playerの初期座標
    public float px;
    public float py;
    public float pz;

    // bombの座標
    public float i;
    public float j;
    public float k;

    // bomb2の座標
    public float i2;
    public float j2;
    public float k2;

    // bomb3の座標
    public float i3;
    public float j3;
    public float k3;

    private Animator anim;

    private bool isRunning = false;
    private bool isNunning = false;

    public int waitingTime;


    // Use this for initialization
    // 西瓜1つ，爆弾を3つ生成
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("swing", false);
        Instantiate(waterM, waterM.transform.position, Quaternion.identity);
        Instantiate(bomb1, new Vector3(i, j, k), Quaternion.identity);
        Instantiate(bomb2, new Vector3(i2, j2, k2), Quaternion.identity);
        Instantiate(bomb3, new Vector3(i3, j3, k3), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //  var hi = GameObject.FindGameObjectsWithTag("rightArm");

        // KinectController.Objectに存在するPlayerMotion.Scriptを取得
        PlayerMotion pm = kc.GetComponent<PlayerMotion>();

        // batを振る部分
        if (flag == 1 && pm.isShaking() == true)
        //   if (flag == 1 && Input.GetKey(KeyCode.B))
        {

            // Debug.Log("bat振っちゃった");
            transform.localRotation = Quaternion.Euler(transform.localRotation.x, transform.localRotation.y + lpy, transform.localRotation.z);
            anim.SetBool("swing", true);

            StartCoroutine("SwingDelay");

            // バットを振ることが出来る回数が0になった時
            if (sCounter == 1)
            {

                // ゲーム終了
                flag = -1;
                StartCoroutine("NextScene");

            }
        }

        // 爆発効果, 西瓜を削除して両方を再生成，もう一度回転する
        else if ( /*Input.GetKey(KeyCode.Space) &&  */flag == 0)
        {

            // checkHit(pPoint);
            //  getPoint = pPoint;

            // Debug.Log("リア充度：" + getPoint);

            anim.SetBool("swing", false);
            anim.transform.localPosition = new Vector3(px, py, pz);

            StartCoroutine("SmallBreak");

        }

        // デフォルト回転
        else if (flag == 1 && pm.isShaking() == false)
        {
            lpy += Time.deltaTime * 180;
            transform.localRotation = Quaternion.Euler(0, lpy, 0);
        }
    }

    // バット振った直後の処理
    IEnumerator SwingDelay()
    {
        if (isRunning)
        {
            yield break;
        }

        isRunning = true;

        // Debug.Log(sCounter);
        yield return new WaitForSeconds(delay);
        flag = 0;
        sCounter--;

        isRunning = false;
    }

    // ゲームの再設定
    IEnumerator SmallBreak()
    {

        if (isNunning)
        {
            yield break;
        }

        isNunning = true;

        yield return new WaitForSeconds(interval);

        // ゲームオブジェクトのタグを取得するよ
        var clones = GameObject.FindGameObjectsWithTag("effect");
        var clones2 = GameObject.FindGameObjectsWithTag("bomb");
        var clones3 = GameObject.FindGameObjectsWithTag("targetWM");

        foreach (var clone in clones)
        {
            Destroy(clone, 0);
        }

        foreach (var clone1 in clones2)
        {
            Destroy(clone1, 0);
        }

        foreach (var clone2 in clones3)
        {
            Destroy(clone2, 0);
        }

        Instantiate(waterM, waterM.transform.position, Quaternion.identity);
        Instantiate(bomb1, bomb1.transform.position, Quaternion.identity);
        Instantiate(bomb2, bomb2.transform.position, Quaternion.identity);
        Instantiate(bomb3, bomb3.transform.position, Quaternion.identity);

        // KinectController.Objectに存在するPlayerMotion.Scriptを取得
        PlayerMotion pm = kc.GetComponent<PlayerMotion>();

        flag = 1;
        pm.Trigger = false;

        isNunning = false;

    }

    // sCounterの値をほかのクラスへ
    public int SwingCount()
    {
        return sCounter;
    }

    // flagの値をほかのクラスへ
    public int FlagTrigger()
    {
        return flag;
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(waitingTime);
        SceneManager.LoadScene("Menu");
    }

    // 点数計算用関数
    /*   void checkHit(int pPoint)
       {

           BombController bc1 = bomb1.GetComponent<BombController>();
           BombController bc2 = bomb2.GetComponent<BombController>();
           BombController bc3 = bomb3.GetComponent<BombController>();

           if (bc1.triggerBomb() == true)
           {

               pPoint = pPoint + bc1.bombPoint();
               bc1.BTrigger = false;
              // Debug.Log(pPoint);

           }
           else if (bc1.triggerBomb() == false)
           {
           }

           if (bc2.triggerBomb() == true)
           {

               pPoint = pPoint + bc2.bombPoint();
               bc2.BTrigger = false;
            //   Debug.Log(pPoint);

           }
           else if (bc2.triggerBomb() == false)
           {

           }

           if (bc3.triggerBomb() == true)
           {

               pPoint = pPoint + bc3.bombPoint();
               bc3.BTrigger = false;
             //  Debug.Log(pPoint);

           }
           else if (bc3.triggerBomb() == false)
           {

           }

           SuicaController sc = meron.GetComponent<SuicaController>();

           if (sc.triggerMeron() == true)
           {
               pPoint = pPoint + sc.suicaPoint();
               sc.STrigger = false;
              // Debug.Log(pPoint);

           }
           else if (sc.triggerMeron() == false)
           {

           }

       } */

}
