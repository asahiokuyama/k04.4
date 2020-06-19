using Sequence = System.Collections.IEnumerator;

/// <summary>
/// ゲームクラス。
/// 学生が編集すべきソースコードです。
/// </summary>
public sealed class Game : GameBase
{
    const int BALL_NUM = 30;
    int[] ball_x = new int[BALL_NUM];
    int[] ball_y = new int[BALL_NUM];
    int[] ball_col = new int[BALL_NUM];
    int[] ball_speed = new int[BALL_NUM];
    int ball_w = 24;
    int ball_h = 24;

    int player_x = 304;
    int player_y = 448;
    int player_speed = 3;
    int player_w = 32;
    int player_h = 32;
    int player_img = 4;

    int score = 0;
    int time = 1800;

    int player_col = 4;
    int combo = 0;

    bool fin = false;
    int intervaltime = 300;
    int[] randomcol = new int[BALL_NUM];
    int judge = 0;
    

    public override void InitGame()
    {
        gc.SetResolution(640, 480);
        for (int i = 0; i < BALL_NUM; i++)
        {
            resetBall(i);
        }
    }

   
    public override void UpdateGame()
    {
        for (int i = 0; i < BALL_NUM; i++)
        {
            //ボールを落とす
            ball_y[i] = ball_y[i] + ball_speed[i];

            //ボールが消えて落ちたら新しく生成
            if (ball_y[i] > 480)
            {
                resetBall(i);
            }

            //ボールとプレイヤーが当たったら
            if (gc.CheckHitRect(ball_x[i], ball_y[i], ball_w, ball_h, player_x, player_y, player_w, player_h))
            {
                if (time >= 0)
                {
                    //得点加算。同じ色が続くとコンボ点が入る
                    //赤玉と黄色玉のみ得点加算
                    if(ball_col[i] == 1||ball_col[i]==2)
                    {
                        score = score + ball_col[i];
                        if (player_col == ball_col[i])
                        {
                            combo++;
                            score += combo;
                        }
                        else
                        {
                            combo = 0;
                        }

                    }
                    //青玉の時はscoreが-50される
                    else
                    {
                        
                       score = score - 50;
                        if(score < 0)
                        {
                            score = 0;
                        }
                  
                    }
                   

                    player_col = ball_col[i];

                }
                resetBall(i);
            }
        }


        //画面をタッチしたらプレイヤー移動
        if (player_x < 600 && player_x > 0)
        {


            if (gc.GetPointerFrameCount(0) > 0)
            {
                if (gc.GetPointerX(0) > 320)
                {
                    player_x += player_speed;
                    player_img = 4;
                }
                else
                {
                    player_x -= player_speed;
                    player_img = 5;
                }
            }

        }
        if (player_x >= 600)
        {
            player_x = player_x - 1;
        }
        if(player_x <= 0)
        {
            player_x = player_x + 1;
        }

        time = time - 1;

        if(fin == true && intervaltime <= 0)
        {
            fin = false;
            time = 1800;
            intervaltime = 300;
            score = 0;
            combo = 0;
            InitGame();
        }


    }

    /// <summary>
    /// 描画の処理
    /// </summary>
    public override void DrawGame()
    {
        gc.ClearScreen();
        if (time >= 0)
        {
            int u = 32 + ((time % 60) / 30) * 32;
            int v = (player_col - 1) * 32;
            gc.DrawClipImage(player_img, player_x, player_y, u, v, 32, 32);
        }
        else
        {
            gc.DrawClipImage(player_img, player_x, player_y, 96, (player_col - 1) * 32, 32, 32);
        }

        //gc.DrawClipImage(player_img, player_x, player_y, 0, 64, 32, 32);
        gc.SetColor(0, 0, 0);
        gc.DrawString("combo:" + combo, 0, 48);
        if (time >= 0)
        {
            gc.DrawString("time:" + time, 0, 0);
        }
        else
        {
            gc.DrawString("finished!!", 0, 0);
            gc.DrawString("あと" + intervaltime + "で再スタート！", 0, 70);
            fin = true;
            intervaltime = intervaltime - 1;

            if(score >= 180)
            {
                gc.DrawString("高スコアおめでとう!!",0,100);
            }else if (score >= 100)
            {
                gc.DrawString("普通のスコア(笑)!", 0, 100);
            }
            else
            {
                gc.DrawString("青に気をつけて!", 0, 100);
            }
           
        }
        gc.DrawString("score:" + score, 0, 24);

        //ボールを描くのはtimeが１以上の時のみ
        if (time >= 0)
        {
            for (int i = 0; i < BALL_NUM; i++)
            {
                gc.DrawImage(ball_col[i], ball_x[i], ball_y[i]);
            }
        }

       
    }

    void resetBall(int id)
    {
        //ボールの位置、速さ、色をランダムで指定
        ball_x[id] = gc.Random(0, 616);
        ball_y[id] = -gc.Random(24, 480);
        ball_speed[id] = gc.Random(3, 6);
        //ball_col[id] = gc.Random(1, 3);

        randomcol[id] = gc.Random(1, 3);
        if(randomcol[id] == 3)
        {
            randomcol[id] = 2;
            judge = judge + 1;
        }
        if(judge > 2)
        {
            randomcol[id] = 3;
            judge = 0;
        }
        ball_col[id] = randomcol[id];
    }

 
}

