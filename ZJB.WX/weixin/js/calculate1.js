var $$ = function (id) {
    return document.getElementById(id);
}
function mytotal() {
    $$('p_1').style.display = 'none';
    $$('myresult').style.display = '';
}
function mytotalsec() {
    $$('t-2').style.display = 'none';
    $$('t-3').style.display = '';;
}
function mytotal_hide() {
    $$('p_1').style.display = '';
    $$('myresult').style.display = 'none';
}
function mytotalsec_hide() {
    $$('t-1').style.display = '';
    $$('t-3').style.display = 'none';;
}
function changeDIV(id) {
    if (id == "1") {
        $$('title01').className = "maintitletd1";
        $$('title02').className = "maintitletda font01";
        $$('title03').className = "maintitletdb font01";
        $$('title04').className = "maintitletda font01";
        $$('title05').className = "maintitletd font01";
    } else if (id == "2") {
        $$('title01').className = "maintitletd font01";
        $$('title02').className = "maintitletda1";
        $$('title03').className = "maintitletdb font01";
        $$('title04').className = "maintitletda font01";
        $$('title05').className = "maintitletd font01";
    } else if (id == "3") {
        $$('title01').className = "maintitletd font01";
        $$('title02').className = "maintitletda font01";
        $$('title03').className = "maintitletdb1";
        $$('title04').className = "maintitletda font01";
        $$('title05').className = "maintitletd font01";
    } else if (id == "4") {
        $$('title01').className = "maintitletd font01";
        $$('title02').className = "maintitletda font01";
        $$('title03').className = "maintitletdb font01";
        $$('title04').className = "maintitletda1";
        $$('title05').className = "maintitletd font01";
    } else if (id == "5") {
        $$('title01').className = "maintitletd font01";
        $$('title02').className = "maintitletda font01";
        $$('title03').className = "maintitletdb font01";
        $$('title04').className = "maintitletda font01";
        $$('title05').className = "maintitletd1";
    }
    for (var i = 1; i < 6; i++) {
        if ($$('calculate0' + i) != null) {
            $$('calculate0' + i).style.display = "none";
        }
        if ($$('note0' + i) != null) {
            $$('note0' + i).style.display = "none";
        }
    }
    $$('calculate0' + id).style.display = "block";
    $$('note0' + id).style.display = "block";
}

//62.180.9

////��������
lilv_array = new Array;

////2004��֮ǰ�ľ�����
//lilv_array[1] = new Array;
//lilv_array[1][1] = new Array;
//lilv_array[1][2] = new Array;
//lilv_array[1][1][5] = 0.0477;//�̴� 1��5�� 4.77%
//lilv_array[1][1][10] = 0.0504;//�̴� 5-30�� 5.04%
//lilv_array[1][2][5] = 0.0360;//������ 1��5�� 3.60%
//lilv_array[1][2][10] = 0.0405;//������ 5-30�� 4.05%

////2005��	1�µ�������
//lilv_array[2] = new Array;
//lilv_array[2][1] = new Array;
//lilv_array[2][2] = new Array;
//lilv_array[2][1][5] = 0.0495;//�̴� 1��5�� 4.95%
//lilv_array[2][1][10] = 0.0531;//�̴� 5-30�� 5.31%
//lilv_array[2][2][5] = 0.0378;//������ 1��5�� 3.78%
//lilv_array[2][2][10] = 0.0423;//������ 5-30�� 4.23%

////2006��	1�µ�����������
//lilv_array[3] = new Array;
//lilv_array[3][1] = new Array;
//lilv_array[3][2] = new Array;
//lilv_array[3][1][5] = 0.0527;//�̴� 1��5�� 5.27%
//lilv_array[3][1][10] = 0.0551;//�̴� 5-30�� 5.51%
//lilv_array[3][2][5] = 0.0396;//������ 1��5�� 3.96%
//lilv_array[3][2][10] = 0.0441;//������ 5-30�� 4.41%

////2006��	1�µ�����������
//lilv_array[4] = new Array;
//lilv_array[4][1] = new Array;
//lilv_array[4][2] = new Array;
//lilv_array[4][1][5] = 0.0527;//�̴� 1��5�� 5.27%
//lilv_array[4][1][10] = 0.0612;//�̴� 5-30�� 6.12%
//lilv_array[4][2][5] = 0.0396;//������ 1��5�� 3.96%
//lilv_array[4][2][10] = 0.0441;//������ 5-30�� 4.41%

////2006��	4��28�յ�����������
//lilv_array[5] = new Array;
//lilv_array[5][1] = new Array;
//lilv_array[5][2] = new Array;
//lilv_array[5][1][5] = 0.0551;//�̴� 1��5�� 5.51%
//lilv_array[5][1][10] = 0.0575;//�̴� 5-30�� 5.75%
//lilv_array[5][2][5] = 0.0414;//������ 1��5�� 4.14%
//lilv_array[5][2][10] = 0.0459;//������ 5-30�� 4.59%

////2006��	4��28�յ�����������
//lilv_array[6] = new Array;
//lilv_array[6][1] = new Array;
//lilv_array[6][2] = new Array;
//lilv_array[6][1][5] = 0.0612;//�̴� 1��5�� 6.12%
//lilv_array[6][1][10] = 0.0639;//�̴� 5-30�� 6.39%
//lilv_array[6][2][5] = 0.0414;//������ 1��5�� 4.14%
//lilv_array[6][2][10] = 0.0459;//������ 5-30�� 4.59%

////2006��	8��19�յ�����������
//lilv_array[7] = new Array;
//lilv_array[7][1] = new Array;
//lilv_array[7][2] = new Array;
//lilv_array[7][1][5] = 0.0551;//�̴� 1��5�� 5.51%
//lilv_array[7][1][10] = 0.0581;//�̴� 5-30�� 5.81%
//lilv_array[7][2][5] = 0.0414;//������ 1��5�� 4.14%
//lilv_array[7][2][10] = 0.0459;//������ 5-30�� 4.59%

////2006��	8��19�յ�����������
//lilv_array[8] = new Array;
//lilv_array[8][1] = new Array;
//lilv_array[8][2] = new Array;
//lilv_array[8][1][5] = 0.0648;//�̴� 1��5�� 6.48%
//lilv_array[8][1][10] = 0.0684;//�̴� 5-30�� 6.84%
//lilv_array[8][2][5] = 0.0414;//������ 1��5�� 4.14%
//lilv_array[8][2][10] = 0.0459;//������ 5-30�� 4.59%


////2007��	3��18�յ�����������
//lilv_array[9] = new Array;
//lilv_array[9][1] = new Array;
//lilv_array[9][2] = new Array;
//lilv_array[9][1][5] = 0.0574;//�̴� 1��5�� 5.74%
//lilv_array[9][1][10] = 0.0604;//�̴� 5-30�� 6.04%
//lilv_array[9][2][5] = 0.0432;//������ 1��5�� 4.32%
//lilv_array[9][2][10] = 0.0477;//������ 5-30�� 4.77%

////2007��	3��18�յ�����������
//lilv_array[10] = new Array;
//lilv_array[10][1] = new Array;
//lilv_array[10][2] = new Array;
//lilv_array[10][1][5] = 0.0675;//�̴� 1��5�� 6.75%
//lilv_array[10][1][10] = 0.0711;//�̴� 5-30�� 7.11%
//lilv_array[10][2][5] = 0.0432;//������ 1��5�� 4.32%
//lilv_array[10][2][10] = 0.0477;//������ 5-30�� 4.77%


////2007��	5��19�յ�����������
//lilv_array[11] = new Array;
//lilv_array[11][1] = new Array;
//lilv_array[11][2] = new Array;
//lilv_array[11][1][5] = 0.0589;//�̴� 1��5�� 5.89%
//lilv_array[11][1][10] = 0.0612;//�̴� 5-30�� 6.12%
//lilv_array[11][2][5] = 0.0441;//������ 1��5�� 4.41%%
//lilv_array[11][2][10] = 0.0486;//������ 5-30�� 4.86%%

////2007��	5��19�յ�����������
//lilv_array[12] = new Array;
//lilv_array[12][1] = new Array;
//lilv_array[12][2] = new Array;
//lilv_array[12][1][5] = 0.0693;//�̴� 1��5�� 6.93%
//lilv_array[12][1][10] = 0.0720;//�̴� 5-30�� 7.20%
//lilv_array[12][2][5] = 0.0441;//������ 1��5�� 4.41%%
//lilv_array[12][2][10] = 0.0486;//������ 5-30�� 4.86%%

////2007��	7��21�յ�����������
//lilv_array[13] = new Array;
//lilv_array[13][1] = new Array;
//lilv_array[13][2] = new Array;
//lilv_array[13][1][5] = 0.0612;//�̴� 1��5�� 6.12%
//lilv_array[13][1][10] = 0.06273;//�̴� 5-30�� 6.273%
//lilv_array[13][2][5] = 0.0450;//������ 1��5�� 4.50%%
//lilv_array[13][2][10] = 0.0495;//������ 5-30�� 4.95%%

////2007��	7��21�յ�����������
//lilv_array[14] = new Array;
//lilv_array[14][1] = new Array;
//lilv_array[14][2] = new Array;
//lilv_array[14][1][5] = 0.0720;//�̴� 1��5�� 7.20%
//lilv_array[14][1][10] = 0.0738;//�̴� 5-30�� 7.38%
//lilv_array[14][2][5] = 0.0450;//������ 1��5�� 4.50%%
//lilv_array[14][2][10] = 0.0495;//������ 5-30�� 4.95%%

////2007��	8��22�յ�����������
//lilv_array[15] = new Array;
//lilv_array[15][1] = new Array;
//lilv_array[15][2] = new Array;
//lilv_array[15][1][5] = 0.06273;//�̴� 1��5�� 6.273%
//lilv_array[15][1][10] = 0.06426;//�̴� 5-30�� 6.426%
//lilv_array[15][2][5] = 0.0459;//������ 1��5�� 4.59%
//lilv_array[15][2][10] = 0.0504;//������ 5-30�� 5.04%

////2007��	8��22�յ�����������
//lilv_array[16] = new Array;
//lilv_array[16][1] = new Array;
//lilv_array[16][2] = new Array;
//lilv_array[16][1][5] = 0.0738;//�̴� 1��5�� 7.38%
//lilv_array[16][1][10] = 0.0756;//�̴� 5-30�� 7.56%
//lilv_array[16][2][5] = 0.0459;//������ 1��5�� 4.59%
//lilv_array[16][2][10] = 0.0504;//������ 5-30�� 5.04%

////2007��	9��15�յ�����������
//lilv_array[17] = new Array;
//lilv_array[17][1] = new Array;
//lilv_array[17][2] = new Array;
//lilv_array[17][1][5] = 0.06503;//�̴� 1��5�� 6.503%
//lilv_array[17][1][10] = 0.06656;//�̴� 5-30�� 6.656%
//lilv_array[17][2][5] = 0.0477;//������ 1��5�� 4.77%
//lilv_array[17][2][10] = 0.0522;//������ 5-30�� 5.22%

////2007��	9��15�յ�����������
//lilv_array[18] = new Array;
//lilv_array[18][1] = new Array;
//lilv_array[18][2] = new Array;
//lilv_array[18][1][5] = 0.0765;//�̴� 1��5�� 7.65%
//lilv_array[18][1][10] = 0.0783;//�̴� 5-30�� 7.83%
//lilv_array[18][2][5] = 0.0477;//������ 1��5�� 4.77%
//lilv_array[18][2][10] = 0.0522;//������ 5-30�� 5.22%

////2007��	9��15��������(�ڶ��׷�)
//lilv_array[19] = new Array;
//lilv_array[19][1] = new Array;
//lilv_array[19][2] = new Array;
//lilv_array[19][1][5] = 0.08415;//�̴� 1��5�� 8.415%
//lilv_array[19][1][10] = 0.08613;//�̴� 5-30�� 8.613%
//lilv_array[19][2][5] = 0.0477;//������ 1��5�� 4.77%
//lilv_array[19][2][10] = 0.0522;//������ 5-30�� 5.22%


////2007��	12��21�յ�����������
//lilv_array[20] = new Array;
//lilv_array[20][1] = new Array;
//lilv_array[20][2] = new Array;
//lilv_array[20][1][5] = 0.06579;//�̴� 1��5�� 6.579%
//lilv_array[20][1][10] = 0.06656;//�̴� 5-30�� 6.656%
//lilv_array[20][2][5] = 0.0477;//������ 1��5�� 4.77%
//lilv_array[20][2][10] = 0.0522;//������ 5-30�� 5.22%

////2007��	12��21�յ�����������
//lilv_array[21] = new Array;
//lilv_array[21][1] = new Array;
//lilv_array[21][2] = new Array;
//lilv_array[21][1][5] = 0.0851;//�̴� 1��5�� 8.514%
//lilv_array[21][1][10] = 0.0861;//�̴� 5-30�� 8.613%
//lilv_array[21][2][5] = 0.0477;//������ 1��5�� 4.77%
//lilv_array[21][2][10] = 0.0522;//������ 5-30�� 5.22%

////2007��	12��21��������(�ڶ��׷�)
//lilv_array[22] = new Array;
//lilv_array[22][1] = new Array;
//lilv_array[22][2] = new Array;
//lilv_array[22][1][5] = 0.0851;//�̴� 1��5�� 8.514%
//lilv_array[22][1][10] = 0.0861;//�̴� 5-30�� 8.613%
//lilv_array[22][2][5] = 0.0477;//������ 1��5�� 4.77%
//lilv_array[22][2][10] = 0.0522;//������ 5-30�� 5.22%

////2008��	09��16������������
//lilv_array[23] = new Array;
//lilv_array[23][1] = new Array;
//lilv_array[23][2] = new Array;
//lilv_array[23][1][5] = 0.06426;//�̴� 1��5�� 6.426%
//lilv_array[23][1][10] = 0.06579;//�̴� 5-30�� 6.579%
//lilv_array[23][2][5] = 0.0459;//������ 1��5�� 4.59%
//lilv_array[23][2][10] = 0.0513;//������ 5-30�� 5.13%

////2008��	09��16������������
//lilv_array[24] = new Array;
//lilv_array[24][1] = new Array;
//lilv_array[24][2] = new Array;
//lilv_array[24][1][5] = 0.0832;//�̴� 1��5�� 8.32%
//lilv_array[24][1][10] = 0.0851;//�̴� 5-30�� 8.51%
//lilv_array[24][2][5] = 0.0459;//������ 1��5�� 4.59%
//lilv_array[24][2][10] = 0.0513;//������ 5-30�� 5.13%

////2008��	09��16��������(�ڶ��׷�)
//lilv_array[25] = new Array;
//lilv_array[25][1] = new Array;
//lilv_array[25][2] = new Array;
//lilv_array[25][1][5] = 0.0832;//�̴� 1��5�� 8.32%
//lilv_array[25][1][10] = 0.0851;//�̴� 5-30�� 8.51%
//lilv_array[25][2][5] = 0.0459;//������ 1��5�� 4.59%
//lilv_array[25][2][10] = 0.0513;//������ 5-30�� 5.13%

////2008��	10��09������������
//lilv_array[26] = new Array;
//lilv_array[26][1] = new Array;
//lilv_array[26][2] = new Array;
//lilv_array[26][1][5] = 0.062;//�̴� 1��5�� 6.20%
//lilv_array[26][1][10] = 0.0635;//�̴� 5-30�� 6.35%
//lilv_array[26][2][5] = 0.0432;//������ 1��5�� 4.32%
//lilv_array[26][2][10] = 0.0486;//������ 5-30�� 4.86%

////2008��	10��09������������
//lilv_array[27] = new Array;
//lilv_array[27][1] = new Array;
//lilv_array[27][2] = new Array;
//lilv_array[27][1][5] = 0.0802;//�̴� 1��5�� 8.02%
//lilv_array[27][1][10] = 0.0822;//�̴� 5-30�� 8.22%
//lilv_array[27][2][5] = 0.0432;//������ 1��5�� 4.32%
//lilv_array[27][2][10] = 0.0486;//������ 5-30�� 4.86%

////2008��	10��09��������(�ڶ��׷�)
//lilv_array[28] = new Array;
//lilv_array[28][1] = new Array;
//lilv_array[28][2] = new Array;
//lilv_array[28][1][5] = 0.0802;//�̴� 1��5�� 8.02%
//lilv_array[28][1][10] = 0.0822;//�̴� 5-30�� 8.22%
//lilv_array[28][2][5] = 0.0432;//������ 1��5�� 4.32%
//lilv_array[28][2][10] = 0.0486;//������ 5-30�� 4.86%

////2008��	10��27������������
//lilv_array[29] = new Array;
//lilv_array[29][1] = new Array;
//lilv_array[29][2] = new Array;
//lilv_array[29][1][5] = 0.051;//�̴� 1��5�� 5.10%
//lilv_array[29][1][10] = 0.0523;//�̴� 5-30�� 5.23%
//lilv_array[29][2][5] = 0.0405;//������ 1��5�� 4.05%
//lilv_array[29][2][10] = 0.0459;//������ 5-30�� 4.59%

////2008��	10��27������������
//lilv_array[30] = new Array;
//lilv_array[30][1] = new Array;
//lilv_array[30][2] = new Array;
//lilv_array[30][1][5] = 0.0802;//�̴� 1��5�� 8.02%
//lilv_array[30][1][10] = 0.0822;//�̴� 5-30�� 8.22%
//lilv_array[30][2][5] = 0.0405;//������ 1��5�� 4.05%
//lilv_array[30][2][10] = 0.0459;//������ 5-30�� 4.59%

////2008��	10��27�������ʻ�׼
//lilv_array[31] = new Array;
//lilv_array[31][1] = new Array;
//lilv_array[31][2] = new Array;
//lilv_array[31][1][5] = 0.0729;//�̴� 1��5�� 7.29%
//lilv_array[31][1][10] = 0.0747;//�̴� 5-30�� 7.47%
//lilv_array[31][2][5] = 0.0405;//������ 1��5�� 4.05%
//lilv_array[31][2][10] = 0.0459;//������ 5-30�� 4.59%

////2008��	10��27��������(�ڶ��׷�)
//lilv_array[32] = new Array;
//lilv_array[32][1] = new Array;
//lilv_array[32][2] = new Array;
//lilv_array[32][1][5] = 0.0802;//�̴� 1��5�� 8.02%
//lilv_array[32][1][10] = 0.0822;//�̴� 5-30�� 8.22%
//lilv_array[32][2][5] = 0.0405;//������ 1��5�� 4.05%
//lilv_array[32][2][10] = 0.0459;//������ 5-30�� 4.59%

////2008��	10��30������������
//lilv_array[33] = new Array;
//lilv_array[33][1] = new Array;
//lilv_array[33][2] = new Array;
//lilv_array[33][1][5] = 0.0491;//�̴� 1��5�� 4.91%
//lilv_array[33][1][10] = 0.0504;//�̴� 5-30�� 5.04%
//lilv_array[33][2][5] = 0.0405;//������ 1��5�� 4.05%
//lilv_array[33][2][10] = 0.0459;//������ 5-30�� 4.59%

////2008��	10��30������������
//lilv_array[34] = new Array;
//lilv_array[34][1] = new Array;
//lilv_array[34][2] = new Array;
//lilv_array[34][1][5] = 0.0772;//�̴� 1��5�� 7.72%
//lilv_array[34][1][10] = 0.0792;//�̴� 5-30�� 7.92%
//lilv_array[34][2][5] = 0.0405;//������ 1��5�� 4.05%
//lilv_array[34][2][10] = 0.0459;//������ 5-30�� 4.59%

////2008��	10��30�������ʻ�׼
//lilv_array[35] = new Array;
//lilv_array[35][1] = new Array;
//lilv_array[35][2] = new Array;
//lilv_array[35][1][5] = 0.0702;//�̴� 1��5�� 7.02%
//lilv_array[35][1][10] = 0.0720;//�̴� 5-30�� 7.20%
//lilv_array[35][2][5] = 0.0405;//������ 1��5�� 4.05%
//lilv_array[35][2][10] = 0.0459;//������ 5-30�� 4.59%

////2008��	10��30��������(�ڶ��׷�)
//lilv_array[36] = new Array;
//lilv_array[36][1] = new Array;
//lilv_array[36][2] = new Array;
//lilv_array[36][1][5] = 0.0772;//�̴� 1��5�� 7.72%
//lilv_array[36][1][10] = 0.0792;//�̴� 5-30�� 7.92%
//lilv_array[36][2][5] = 0.0405;//������ 1��5�� 4.05%
//lilv_array[36][2][10] = 0.0459;//������ 5-30�� 4.59%

////2008��	11��27������������
//lilv_array[37] = new Array;
//lilv_array[37][1] = new Array;
//lilv_array[37][2] = new Array;
//lilv_array[37][1][5] = 0.0416;//�̴� 1��5�� 4.16%
//lilv_array[37][1][10] = 0.0428;//�̴� 5-30�� 4.28%
//lilv_array[37][2][5] = 0.0351;//������ 1��5�� 3.51%
//lilv_array[37][2][10] = 0.0405;//������ 5-30�� 4.05%

////2008��	11��27������������
//lilv_array[38] = new Array;
//lilv_array[38][1] = new Array;
//lilv_array[38][2] = new Array;
//lilv_array[38][1][5] = 0.0653;//�̴� 1��5�� 6.53%
//lilv_array[38][1][10] = 0.0673;//�̴� 5-30�� 6.73%
//lilv_array[38][2][5] = 0.0351;//������ 1��5�� 3.51%
//lilv_array[38][2][10] = 0.0405;//������ 5-30�� 4.05%

////2008��	11��27�������ʻ�׼
//lilv_array[39] = new Array;
//lilv_array[39][1] = new Array;
//lilv_array[39][2] = new Array;
//lilv_array[39][1][5] = 0.0594;//�̴� 1��5�� 5.94%
//lilv_array[39][1][10] = 0.0612;//�̴� 5-30�� 6.12%
//lilv_array[39][2][5] = 0.0351;//������ 1��5�� 3.51%
//lilv_array[39][2][10] = 0.0405;//������ 5-30�� 4.05%

////2008��	11��27��������(�ڶ��׷�)
//lilv_array[40] = new Array;
//lilv_array[40][1] = new Array;
//lilv_array[40][2] = new Array;
//lilv_array[40][1][5] = 0.0653;//�̴� 1��5�� 6.53%
//lilv_array[40][1][10] = 0.0673;//�̴� 5-30�� 6.73%
//lilv_array[40][2][5] = 0.0351;//������ 1��5�� 3.51%
//lilv_array[40][2][10] = 0.0405;//������ 5-30�� 4.05%

////2008��	12��23������������(7��)
//lilv_array[41] = new Array;
//lilv_array[41][1] = new Array;
//lilv_array[41][2] = new Array;
//lilv_array[41][1][5] = 0.0403;//�̴� 1��5�� 4.03%
//lilv_array[41][1][10] = 0.0416;//�̴� 5-30�� 4.16%
//lilv_array[41][2][5] = 0.0333;//������ 1��5�� 3.33%
//lilv_array[41][2][10] = 0.0387;//������ 5-30�� 3.87%

////2008��	12��23������������(85��)
//lilv_array[42] = new Array;
//lilv_array[42][1] = new Array;
//lilv_array[42][2] = new Array;
//lilv_array[42][1][5] = 0.0490;//�̴� 1��5�� 4.90%
//lilv_array[42][1][10] = 0.0505;//�̴� 5-30�� 5.05%
//lilv_array[42][2][5] = 0.0333;//������ 1��5�� 3.33%
//lilv_array[42][2][10] = 0.0387;//������ 5-30�� 3.87%

////2008��	12��23������������(1.1��)
//lilv_array[43] = new Array;
//lilv_array[43][1] = new Array;
//lilv_array[43][2] = new Array;
//lilv_array[43][1][5] = 0.0634;//�̴� 1��5�� 6.34%
//lilv_array[43][1][10] = 0.0653;//�̴� 5-30�� 6.53%
//lilv_array[43][2][5] = 0.0333;//������ 1��5�� 3.33%
//lilv_array[43][2][10] = 0.0387;//������ 5-30�� 3.87%

////2008��	12��23�������ʻ�׼
//lilv_array[44] = new Array;
//lilv_array[44][1] = new Array;
//lilv_array[44][2] = new Array;
//lilv_array[44][1][5] = 0.0576;//�̴� 1��5�� 5.76%
//lilv_array[44][1][10] = 0.0594;//�̴� 5-30�� 5.94%
//lilv_array[44][2][5] = 0.0333;//������ 1��5�� 3.33%
//lilv_array[44][2][10] = 0.0387;//������ 5-30�� 3.87%

////2008��	12��23��������(�ڶ��׷�)(1.1��)
//lilv_array[45] = new Array;
//lilv_array[45][1] = new Array;
//lilv_array[45][2] = new Array;
//lilv_array[45][1][5] = 0.0634;//�̴� 1��5�� 6.34%
//lilv_array[45][1][10] = 0.0653;//�̴� 5-30�� 6.53%
//lilv_array[45][2][5] = 0.0333;//������ 1��5�� 3.33%
//lilv_array[45][2][10] = 0.0387;//������ 5-30�� 3.87%

////2010��	10��20����������(7��)
//lilv_array[46]=new Array;
//lilv_array[46][1]=new Array;
//lilv_array[46][2]=new Array;
//lilv_array[46][1][5]=0.0596*0.7;//�̴� 1��5�� 5.76%    
//lilv_array[46][1][10]=0.0614*0.7;//�̴� 5-30�� 5.94%   
//lilv_array[46][2][5]=0.0350;//������ 1��5�� 3.50%  
//lilv_array[46][2][10]=0.0405;//������ 5-30�� 4.05% 

////2010��	10��20����������(85��)
//lilv_array[47]=new Array;
//lilv_array[47][1]=new Array;
//lilv_array[47][2]=new Array;
//lilv_array[47][1][5]=0.0596*0.85;//�̴� 1��5�� 5.76%    
//lilv_array[47][1][10]=0.0614*0.85;//�̴� 5-30�� 5.94%   
//lilv_array[47][2][5]=0.0350;//������ 1��5�� 3.50%  
//lilv_array[47][2][10]=0.0405;//������ 5-30�� 4.05% 

////2010��	10��20�ջ�׼����
//lilv_array[48]=new Array;
//lilv_array[48][1]=new Array;
//lilv_array[48][2]=new Array;
//lilv_array[48][1][5]=0.0596;//�̴� 1��5�� 5.76%
//lilv_array[48][1][10]=0.0614;//�̴� 5-30�� 5.94%
//lilv_array[48][2][5]=0.0350;//������ 1��5�� 3.50%  
//lilv_array[48][2][10]=0.0405;//������ 5-30�� 4.05% 

////2010��	10��20����������(1.1��)
//lilv_array[49]=new Array;
//lilv_array[49][1]=new Array;
//lilv_array[49][2]=new Array;
//lilv_array[49][1][5]=0.0596*1.1;//�̴� 1��5�� 5.76%    
//lilv_array[49][1][10]=0.0614*1.1;//�̴� 5-30�� 5.94%   
//lilv_array[49][2][5]=0.0350;//������ 1��5�� 3.50%  
//lilv_array[49][2][10]=0.0405;//������ 5-30�� 4.05% 


////10��12��26�ջ�׼����
//lilv_array[9] = new Array;
//lilv_array[9][1] = new Array;
//lilv_array[9][2] = new Array;
//lilv_array[9][1][5] = 0.0622;//�̴� 1��5�� 6.22%
//lilv_array[9][1][10] = 0.0640;//�̴� 5-30�� 6.4%
//lilv_array[9][2][5] = 0.0375;//������ 1��5�� 3.75%
//lilv_array[9][2][10] = 0.0430;//������ 5-30�� 4.3%
////10��12��26����������(7��)
//lilv_array[10] = new Array;
//lilv_array[10][1] = new Array;
//lilv_array[10][2] = new Array;
//lilv_array[10][1][5] = 0.04354;//�̴� 1��5�� 4.354%
//lilv_array[10][1][10] = 0.0448;//�̴� 5-30�� 4.48%
//lilv_array[10][2][5] = 0.0375;//������ 1��5�� 3.75%
//lilv_array[10][2][10] = 0.0430;//������ 5-30�� 4.3%
////10��12��26����������(1.1��)
//lilv_array[11] = new Array;
//lilv_array[11][1] = new Array;
//lilv_array[11][2] = new Array;
//lilv_array[11][1][5] = 0.06842;//�̴� 1��5�� 6.842%
//lilv_array[11][1][10] = 0.0704;//�̴� 5-30�� 7.04%
//lilv_array[11][2][5] = 0.0375;//������ 1��5�� 3.75%
//lilv_array[11][2][10] = 0.0430;//������ 5-30�� 4.3%
////11��2��9�ջ�׼����
//lilv_array[12] = new Array;
//lilv_array[12][1] = new Array;
//lilv_array[12][2] = new Array;
//lilv_array[12][1][5] = 0.0645;//�̴� 1��5�� 6.45%
//lilv_array[12][1][10] = 0.0660;//�̴� 5-30�� 6.6%
//lilv_array[12][2][5] = 0.0400;//������ 1��5�� 4%
//lilv_array[12][2][10] = 0.0450;//������ 5-30�� 4.5%
////11��2��9����������(7��)
//lilv_array[13] = new Array;
//lilv_array[13][1] = new Array;
//lilv_array[13][2] = new Array;
//lilv_array[13][1][5] = 0.04515;//�̴� 1��5�� 4.515%
//lilv_array[13][1][10] = 0.04620;//�̴� 5-30�� 4.62%
//lilv_array[13][2][5] = 0.0400;//������ 1��5�� 4%
//lilv_array[13][2][10] = 0.0450;//������ 5-30�� 4.5%
////11��2��9����������(85��)
//lilv_array[14] = new Array;
//lilv_array[14][1] = new Array;
//lilv_array[14][2] = new Array;
//lilv_array[14][1][5] = 0.054825;//�̴� 1��5�� 5.4825%
//lilv_array[14][1][10] = 0.0561;//�̴� 5-30�� 5.61%
//lilv_array[14][2][5] = 0.0400;//������ 1��5�� 4%
//lilv_array[14][2][10] = 0.0450;//������ 5-30�� 4.5%
////11��2��9����������(1.1��)
//lilv_array[15] = new Array;
//lilv_array[15][1] = new Array;
//lilv_array[15][2] = new Array;
//lilv_array[15][1][5] = 0.07095;//�̴� 1��5�� 7.095%
//lilv_array[15][1][10] = 0.0726;//�̴� 5-30�� 7.26%
//lilv_array[15][2][5] = 0.0400;//������ 1��5�� 4%
//lilv_array[15][2][10] = 0.0450;//������ 5-30�� 4.5%
////11��4��5�ջ�׼����
//lilv_array[16] = new Array;
//lilv_array[16][1] = new Array;
//lilv_array[16][2] = new Array;
//lilv_array[16][1][5] = 0.0665;//�̴� 1��5�� 6.65%
//lilv_array[16][1][10] = 0.0680;//�̴� 5-30�� 6.8%
//lilv_array[16][2][5] = 0.0420;//������ 1��5�� 4.2%
//lilv_array[16][2][10] = 0.0470;//������ 5-30�� 4.7%
////11��4��5���������ޣ�7�ۣ�
//lilv_array[17] = new Array;
//lilv_array[17][1] = new Array;
//lilv_array[17][2] = new Array;
//lilv_array[17][1][5] = 0.04655;//�̴� 1��5�� 4.655%
//lilv_array[17][1][10] = 0.0476;//�̴� 5-30�� 4.76%
//lilv_array[17][2][5] = 0.0420;//������ 1��5�� 4.2%
//lilv_array[17][2][10] = 0.0470;//������ 5-30�� 4.7%
////11��4��5���������ޣ�85�ۣ�
//lilv_array[18] = new Array;
//lilv_array[18][1] = new Array;
//lilv_array[18][2] = new Array;
//lilv_array[18][1][5] = 0.056525;//�̴� 1��5�� 5.6525%
//lilv_array[18][1][10] = 0.0578;//�̴� 5-30�� 5.78%
//lilv_array[18][2][5] = 0.0420;//������ 1��5�� 4.2%
//lilv_array[18][2][10] = 0.0470;//������ 5-30�� 4.7%
////11��4��5���������ޣ�1.1����
//lilv_array[19] = new Array;
//lilv_array[19][1] = new Array;
//lilv_array[19][2] = new Array;
//lilv_array[19][1][5] = 0.07315;//�̴� 1��5�� 7.315%
//lilv_array[19][1][10] = 0.0748;//�̴� 5-30�� 7.48%
//lilv_array[19][2][5] = 0.0420;//������ 1��5�� 4.2%
//lilv_array[19][2][10] = 0.0470;//������ 5-30�� 4.7%
////11��7��6�ջ�׼����
//lilv_array[20] = new Array;
//lilv_array[20][1] = new Array;
//lilv_array[20][2] = new Array;
//lilv_array[20][1][5] = 0.0690;//�̴� 1��5�� 6.9%
//lilv_array[20][1][10] = 0.0705;//�̴� 5-30�� 7.05%
//lilv_array[20][2][5] = 0.0445;//������ 1��5�� 4.45%
//lilv_array[20][2][10] = 0.0490;//������ 5-30�� 4.9%
////11��7��6���������ޣ�7�ۣ�
//lilv_array[21] = new Array;
//lilv_array[21][1] = new Array;
//lilv_array[21][2] = new Array;
//lilv_array[21][1][5] = 0.0483;//�̴� 1��5�� 4.83%
//lilv_array[21][1][10] = 0.04935;//�̴� 5-30�� 4.935%
//lilv_array[21][2][5] = 0.0445;//������ 1��5�� 4.45%
//lilv_array[21][2][10] = 0.0490;//������ 5-30�� 4.9%
////11��7��6���������ޣ�85�ۣ�
//lilv_array[22] = new Array;
//lilv_array[22][1] = new Array;
//lilv_array[22][2] = new Array;
//lilv_array[22][1][5] = 0.05865;//�̴� 1��5�� 5.865%
//lilv_array[22][1][10] = 0.059925;//�̴� 5-30�� 5.9925%
//lilv_array[22][2][5] = 0.0445;//������ 1��5�� 4.45%
//lilv_array[22][2][10] = 0.0490;//������ 5-30�� 4.9%
////11��7��6���������ޣ�1.1����
//lilv_array[23] = new Array;
//lilv_array[23][1] = new Array;
//lilv_array[23][2] = new Array;
//lilv_array[23][1][5] = 0.0759;//�̴� 1��5�� 7.59%
//lilv_array[23][1][10] = 0.07755;//�̴� 5-30�� 7.755%
//lilv_array[23][2][5] = 0.0445;//������ 1��5�� 4.45%
//lilv_array[23][2][10] = 0.0490;//������ 5-30�� 4.9%


////12��6��8�ջ�׼����

//lilv_array[24] = new Array;

//lilv_array[24][1] = new Array;

//lilv_array[24][2] = new Array;

//lilv_array[24][1][5] = 0.0665;//�̴� 1��5�� 6.65%

//lilv_array[24][1][10] = 0.0680;//�̴� 5-30�� 6.8%

//lilv_array[24][2][5] = 0.0420;//������ 1��5�� 4.2%

//lilv_array[24][2][10] = 0.0470;//������ 5-30�� 4.7%

////12��6��8���������ޣ�7�ۣ�

//lilv_array[25] = new Array;

//lilv_array[25][1] = new Array;

//lilv_array[25][2] = new Array;

//lilv_array[25][1][5] = 0.04655;//�̴� 1��5�� 4.655%

//lilv_array[25][1][10] = 0.0476;//�̴� 5-30�� 4.76%

//lilv_array[25][2][5] = 0.0420;//������ 1��5�� 4.2%

//lilv_array[25][2][10] = 0.0470;//������ 5-30�� 4.7%

////12��6��8���������ޣ�85�ۣ�

//lilv_array[26] = new Array;

//lilv_array[26][1] = new Array;

//lilv_array[26][2] = new Array;

//lilv_array[26][1][5] = 0.056525;//�̴� 1��5�� 5.6525%

//lilv_array[26][1][10] = 0.0578;//�̴� 5-30�� 5.78%

//lilv_array[26][2][5] = 0.0420;//������ 1��5�� 4.2%

//lilv_array[26][2][10] = 0.0470;//������ 5-30�� 4.7%

////12��6��8���������ޣ�1.1����

//lilv_array[27] = new Array;

//lilv_array[27][1] = new Array;

//lilv_array[27][2] = new Array;

//lilv_array[27][1][5] = 0.07315;//�̴� 1��5�� 7.315%

//lilv_array[27][1][10] = 0.0748;//�̴� 5-30�� 7.48%

//lilv_array[27][2][5] = 0.0420;//������ 1��5�� 4.2%

//lilv_array[27][2][10] = 0.0470;//������ 5-30�� 4.7%

////12��7��6�ջ�׼����

//lilv_array[28] = new Array;

//lilv_array[28][1] = new Array;

//lilv_array[28][2] = new Array;

//lilv_array[28][1][5] = 0.0640;//�̴� 1��5�� 6.4%

//lilv_array[28][1][10] = 0.0655;//�̴� 5-30�� 6.55%

//lilv_array[28][2][5] = 0.0400;//������ 1��5�� 4%

//lilv_array[28][2][10] = 0.0450;//������ 5-30�� 4.5%

////12��7��6���������ޣ�7�ۣ�

//lilv_array[29] = new Array;

//lilv_array[29][1] = new Array;

//lilv_array[29][2] = new Array;

//lilv_array[29][1][5] = 0.0448;//�̴� 1��5�� 4.48%

//lilv_array[29][1][10] = 0.04585;//�̴� 5-30�� 4.585%

//lilv_array[29][2][5] = 0.0400;//������ 1��5�� 4%

//lilv_array[29][2][10] = 0.0450;//������ 5-30�� 4.5%

////12��7��6���������ޣ�85�ۣ�

//lilv_array[30] = new Array;

//lilv_array[30][1] = new Array;

//lilv_array[30][2] = new Array;

//lilv_array[30][1][5] = 0.0544;//�̴� 1��5�� 5.44%

//lilv_array[30][1][10] = 0.055675;//�̴� 5-30�� 5.5675%

//lilv_array[30][2][5] = 0.0400;//������ 1��5�� 4%

//lilv_array[30][2][10] = 0.0450;//������ 5-30�� 4.5%

////12��7��6���������ޣ�1.1����

//lilv_array[31] = new Array;

//lilv_array[31][1] = new Array;

//lilv_array[31][2] = new Array;

//lilv_array[31][1][5] = 0.0704;//�̴� 1��5�� 7.04%

//lilv_array[31][1][10] = 0.07205;//�̴� 5-30�� 7.205%

//lilv_array[31][2][5] = 0.0400;//������ 1��5�� 4%

//lilv_array[31][2][10] = 0.0450;//������ 5-30�� 4.5%

//14��11��22���������޻�׼����

lilv_array[32] = new Array;

lilv_array[32][1] = new Array;

lilv_array[32][2] = new Array;

lilv_array[32][1][1] = 0.0560;//�̴� 1�� 5.6%

lilv_array[32][1][5] = 0.0600;//�̴� 2��5�� 6.0%

lilv_array[32][1][10] = 0.0615;//�̴� 5-30�� 6.15%

lilv_array[32][2][5] = 0.03750;//������ 1��5�� 3.75%

lilv_array[32][2][10] = 0.0425;//������ 5-30�� 4.25%

//14��11��22���������ޣ�85�ۣ�

lilv_array[33] = new Array;

lilv_array[33][1] = new Array;

lilv_array[33][2] = new Array;

lilv_array[33][1][1] = 0.04760;//�̴� 1�� 4.76%

lilv_array[33][1][5] = 0.051;//�̴� 2��5�� 5.10%

lilv_array[33][1][10] = 0.0523;//�̴� 5-30�� 5.23%

lilv_array[33][2][5] = 0.0375;//������ 1��5�� 3.75%

lilv_array[33][2][10] = 0.0425;//������ 5-30�� 4.25%

//14��11��22���������ޣ�9�ۣ�

lilv_array[34] = new Array;

lilv_array[34][1] = new Array;

lilv_array[34][2] = new Array;

lilv_array[34][1][1] = 0.0504;//�̴� 1�� 5.04%

lilv_array[34][1][5] = 0.054;//�̴� 1��5�� 5.4%

lilv_array[34][1][10] = 0.0554;//�̴� 5-30�� 5.54%

lilv_array[34][2][5] = 0.0375;//������ 1��5�� 3.75%

lilv_array[34][2][10] = 0.0425;//������ 5-30�� 4.25%

//14��11��22���������ޣ�1.1����

lilv_array[35] = new Array;

lilv_array[35][1] = new Array;

lilv_array[35][2] = new Array;

lilv_array[35][1][1] = 0.06160;//�̴� 1�� 6.16%

lilv_array[35][1][5] = 0.066;//�̴� 1��5�� 6.6%

lilv_array[35][1][10] = 0.0677;//�̴� 5-30�� 6.77%

lilv_array[35][2][5] = 0.0375;//������ 1��5�� 3.75%

lilv_array[35][2][10] = 0.0425;//������ 5-30�� 4.25%

//15��10��24�ջ�׼������
lilv_array[36] = new Array;
lilv_array[36][1] = new Array;
lilv_array[36][2] = new Array;
lilv_array[36][1][1] = 0.0435;//�̴� 1�� 4.35%
lilv_array[36][1][5] = 0.0475;//�̴� 1��5�� 4.75%
lilv_array[36][1][10] = 0.0490;//�̴� 5-30�� 6.77%
lilv_array[36][2][1] = 0.0275;//������ 1��5�� 2.75%
lilv_array[36][2][5] = 0.0275;//������ 1��5�� 2.75%
lilv_array[36][2][10] = 0.0325;//������ 5-30�� 3.25%


//15��10��24���������ޣ�7�ۣ�
lilv_array[37] = new Array;
lilv_array[37][1] = new Array;
lilv_array[37][2] = new Array;
lilv_array[37][1][1] = 0.030450;//�̴� 1�� 3.0450%
lilv_array[37][1][5] = 0.033250;//�̴� 1��5�� 3.3250%
lilv_array[37][1][10] = 0.034300;//�̴� 5-30�� 3.4300%
lilv_array[37][2][1] = 0.019250;//������ 1��5�� 1.9250%
lilv_array[37][2][5] = 0.019250;//������ 1��5�� 1.9250%
lilv_array[37][2][10] = 0.022750;//������ 5-30�� 2.2750%


//15��10��24���������ޣ�8.5�ۣ�
lilv_array[38] = new Array;
lilv_array[38][1] = new Array;
lilv_array[38][2] = new Array;
lilv_array[38][1][1] = 0.036975;//�̴� 1�� 3.6975%
lilv_array[38][1][5] = 0.040375;//�̴� 1��5��4.0375%
lilv_array[38][1][10] = 0.041650;//�̴� 5-30��4.1650%
lilv_array[38][2][1] = 0.023375;//������ 1��5�� 2.3375%
lilv_array[38][2][5] = 0.023375;//������ 1��5�� 2.3375%
lilv_array[38][2][10] = 0.027625;//������ 5-30�� 2.7625%

//15��10��24���������ޣ�1.1��
lilv_array[39] = new Array;
lilv_array[39][1] = new Array;
lilv_array[39][2] = new Array;
lilv_array[39][1][1] = 0.047850;//�̴� 1�� 4.7850%
lilv_array[39][1][5] = 0.052250;//�̴� 1��5��5.2250%
lilv_array[39][1][10] = 0.053900;//�̴� 5-30�� 5.3900%
lilv_array[39][2][1] = 0.030250;//������ 1��5�� 3.0250%
lilv_array[39][2][5] = 0.030250;//������ 1��5�� 3.0250%
lilv_array[39][2][10] = 0.035750;//������ 5-30�� 3.5750%







//2015��3��1�ջ�׼����
lilv_array[13] = new Array;
lilv_array[13][1] = new Array;
lilv_array[13][2] = new Array;
lilv_array[13][1][1] = 0.0535;//�̴�1�� 6%
lilv_array[13][1][3] = 0.0575;//�̴�1��3�� 6%
lilv_array[13][1][5] = 0.0575;//�̴� 3��5�� 6%
lilv_array[13][1][10] = 0.0590;//�̴� 5-30�� 6.15%
lilv_array[13][2][5] = 0.0350;//������ 1��5�� 4%
lilv_array[13][2][10] = 0.0400;//������ 5-30�� 4.5%
///2015��3��1���������ޣ�7�ۣ�
lilv_array[14] = new Array;
lilv_array[14][1] = new Array;
lilv_array[14][2] = new Array;
lilv_array[14][1][1] = 0.03745;//�̴�1�� 6%
lilv_array[14][1][3] = 0.04025;//�̴�1��3�� 6%
lilv_array[14][1][5] = 0.04025;//�̴� 3��5�� 6%
lilv_array[14][1][10] = 0.04130;//�̴� 5-30�� 6.15%
lilv_array[14][2][5] = 0.02450;//������ 1��5�� 4%
lilv_array[14][2][10] = 0.02800;//������ 5-30�� 4.5%
///2015��3��1���������ޣ�85�ۣ�
lilv_array[15] = new Array;
lilv_array[15][1] = new Array;
lilv_array[15][2] = new Array;
lilv_array[15][1][1] = 0.045475; //�̴�1�� 5.1%
lilv_array[15][1][3] = 0.0488750; //�̴�1��3�� 5.2275%
lilv_array[15][1][5] = 0.0488750; //�̴� 3��5�� 5.44%
lilv_array[15][1][10] = 0.05015; //�̴� 5-30�� 5.5675%
lilv_array[15][2][5] = 0.02975; //������ 1��5�� 4%
lilv_array[15][2][10] = 0.03400; //������ 5-30�� 4.5%
///2015��3��1���������ޣ�1.1����
lilv_array[16] = new Array;
lilv_array[16][1] = new Array;
lilv_array[16][2] = new Array;
lilv_array[16][1][1] = 0.05885; //�̴�1�� 6.6%
lilv_array[16][1][3] = 0.06325; //�̴�1��3�� 6.765%
lilv_array[16][1][5] = 0.06325; //�̴� 3��5�� 7.04%
lilv_array[16][1][10] = 0.0649; //�̴� 5-30�� 7.205%
lilv_array[16][2][5] = 0.03850; //������ 1��5�� 4%
lilv_array[16][2][10] = 0.0440; //������ 5-30�� 4.5%

//2015��5��11�ջ�׼����

lilv_array[17] = new Array;

lilv_array[17][1] = new Array;

lilv_array[17][2] = new Array;

lilv_array[17][1][1] = 0.0510;//�̴�1�� 6%

lilv_array[17][1][3] = 0.0550;//�̴�1��3�� 6%

lilv_array[17][1][5] = 0.0550;//�̴� 3��5�� 6%

lilv_array[17][1][10] = 0.0565;//�̴� 5-30�� 6.15%

lilv_array[17][2][5] = 0.03250;//������ 1��5�� 4%

lilv_array[17][2][10] = 0.03750;//������ 5-30�� 4.5%

//2015��5��11���������ޣ�7�ۣ�

lilv_array[18] = new Array;

lilv_array[18][1] = new Array;

lilv_array[18][2] = new Array;

lilv_array[18][1][1] = 0.0357;//�̴�1�� 6%

lilv_array[18][1][3] = 0.0385;//�̴�1��3�� 6%

lilv_array[18][1][5] = 0.0385;//�̴� 3��5�� 6%

lilv_array[18][1][10] = 0.03955;//�̴� 5-30�� 6.15%

lilv_array[18][2][5] = 0.02275;//������ 1��5�� 4%

lilv_array[18][2][10] = 0.02625;//������ 5-30�� 4.5%

//2015��5��11���������ޣ�85�ۣ�

lilv_array[19] = new Array;

lilv_array[19][1] = new Array;

lilv_array[19][2] = new Array;

lilv_array[19][1][1] = 0.04335; //�̴�1�� 5.1%

lilv_array[19][1][3] = 0.04675; //�̴�1��3�� 5.2275%

lilv_array[19][1][5] = 0.04675; //�̴� 3��5�� 5.44%

lilv_array[19][1][10] = 0.048025; //�̴� 5-30�� 5.5675%

lilv_array[19][2][5] = 0.027625; //������ 1��5�� 4%

lilv_array[19][2][10] = 0.031875; //������ 5-30�� 4.5%

//2015��5��11���������ޣ�1.1����

lilv_array[20] = new Array;

lilv_array[20][1] = new Array;

lilv_array[20][2] = new Array;

lilv_array[20][1][1] = 0.0561; //�̴�1�� 6.6%

lilv_array[20][1][3] = 0.0605; //�̴�1��3�� 6.765%

lilv_array[20][1][5] = 0.0605; //�̴� 3��5�� 7.04%

lilv_array[20][1][10] = 0.06215; //�̴� 5-30�� 7.205%

lilv_array[20][2][5] = 0.03575; //������ 1��5�� 4%

lilv_array[20][2][10] = 0.04125; //������ 5-30�� 4.5%


//2015��6��28�ջ�׼����

lilv_array[21] = new Array;

lilv_array[21][1] = new Array;

lilv_array[21][2] = new Array;

lilv_array[21][1][1] = 0.0485;//�̴�1�� 6%

lilv_array[21][1][3] = 0.0525;//�̴�1��3�� 6%

lilv_array[21][1][5] = 0.0525;//�̴� 3��5�� 6%

lilv_array[21][1][10] = 0.0540;//�̴� 5-30�� 6.15%

lilv_array[21][2][5] = 0.0300;//������ 1��5�� 4%

lilv_array[21][2][10] = 0.0350;//������ 5-30�� 4.5%

//2015��6��28���������ޣ�7�ۣ�

lilv_array[22] = new Array;

lilv_array[22][1] = new Array;

lilv_array[22][2] = new Array;

lilv_array[22][1][1] = 0.0340;//�̴�1�� 6%

lilv_array[22][1][3] = 0.0368;//�̴�1��3�� 6%

lilv_array[22][1][5] = 0.0368;//�̴� 3��5�� 6%

lilv_array[22][1][10] = 0.0378;//�̴� 5-30�� 6.15%

lilv_array[22][2][5] = 0.0210;//������ 1��5�� 4%

lilv_array[22][2][10] = 0.0245;//������ 5-30�� 4.5%

//2015��6��28���������ޣ�85�ۣ�

lilv_array[23] = new Array;

lilv_array[23][1] = new Array;

lilv_array[23][2] = new Array;

lilv_array[23][1][1] = 0.0412; //�̴�1�� 5.1%

lilv_array[23][1][3] = 0.0446; //�̴�1��3�� 5.2275%

lilv_array[23][1][5] = 0.0446; //�̴� 3��5�� 5.44%

lilv_array[23][1][10] = 0.0459; //�̴� 5-30�� 5.5675%

lilv_array[23][2][5] = 0.0255; //������ 1��5�� 4%

lilv_array[23][2][10] = 0.0297; //������ 5-30�� 4.5%

//2015��6��28���������ޣ�1.1����

lilv_array[24] = new Array;

lilv_array[24][1] = new Array;

lilv_array[24][2] = new Array;

lilv_array[24][1][1] = 0.0534; //�̴�1�� 6.6%

lilv_array[24][1][3] = 0.0578; //�̴�1��3�� 6.765%

lilv_array[24][1][5] = 0.0578; //�̴� 3��5�� 7.04%

lilv_array[24][1][10] = 0.0594; //�̴� 5-30�� 7.205%

lilv_array[24][2][5] = 0.0330; //������ 1��5�� 4%

lilv_array[24][2][10] = 0.0385; //������ 5-30�� 4.5%

//2015��8��26�ջ�׼����

lilv_array[25] = new Array;

lilv_array[25][1] = new Array;

lilv_array[25][2] = new Array;

lilv_array[25][1][1] = 0.0460;//�̴�1�� 6%

lilv_array[25][1][3] = 0.0500;//�̴�1��3�� 6%

lilv_array[25][1][5] = 0.0500;//�̴� 3��5�� 6%

lilv_array[25][1][10] = 0.0515;//�̴� 5-30�� 6.15%

lilv_array[25][2][5] = 0.0275;//������ 1��5�� 4%

lilv_array[25][2][10] = 0.0325;//������ 5-30�� 4.5%

//2015��8��26���������ޣ�7�ۣ�

lilv_array[26] = new Array;

lilv_array[26][1] = new Array;

lilv_array[26][2] = new Array;

lilv_array[26][1][1] = 0.0322;//�̴�1�� 6%

lilv_array[26][1][3] = 0.0350;//�̴�1��3�� 6%

lilv_array[26][1][5] = 0.0350;//�̴� 3��5�� 6%

lilv_array[26][1][10] = 0.03605;//�̴� 5-30�� 6.15%

lilv_array[26][2][5] = 0.01925;//������ 1��5�� 4%

lilv_array[26][2][10] = 0.02275;//������ 5-30�� 4.5%

//2015��8��26���������ޣ�85�ۣ�

lilv_array[27] = new Array;

lilv_array[27][1] = new Array;

lilv_array[27][2] = new Array;

lilv_array[27][1][1] = 0.0391; //�̴�1�� 5.1%

lilv_array[27][1][3] = 0.0425; //�̴�1��3�� 5.2675%

lilv_array[27][1][5] = 0.0425; //�̴� 3��5�� 5.44%

lilv_array[27][1][10] = 0.043775; //�̴� 5-30�� 5.5675%

lilv_array[27][2][5] = 0.023375; //������ 1��5�� 4%

lilv_array[27][2][10] = 0.027625; //������ 5-30�� 4.5%

//2015��8��26���������ޣ�1.1����

lilv_array[28] = new Array;

lilv_array[28][1] = new Array;

lilv_array[28][2] = new Array;

lilv_array[28][1][1] = 0.0506; //�̴�1�� 6.6%

lilv_array[28][1][3] = 0.0550; //�̴�1��3�� 6.765%

lilv_array[28][1][5] = 0.0550; //�̴� 3��5�� 7.04%

lilv_array[28][1][10] = 0.05665; //�̴� 5-30�� 7.205%

lilv_array[28][2][5] = 0.03025; //������ 1��5�� 4%

lilv_array[28][2][10] = 0.03575; //������ 5-30�� 4.5%


function myround(v, e) {
    var t = 1;
    e = Math.round(e);
    for (; e > 0; t *= 10, e--);
    for (; e < 0; t /= 10, e++);
    return Math.round(v * t) / t;
}

function ShowLilv(obj, month, lt, n) {

    var year = 1;

    if (month != 1) {
        year =
        (month > 5) ? 10 : 5;
    }


    $("#sdlv").val(myround(lilv_array[lt][1][year] * 100, 2));
    $("#gjlv").val(myround(lilv_array[lt][2][year] * 100, 2));

    if (n == 1) {
        $("#years").html($(obj).html());
        $("#years").attr("data", month)
    }
    else {
        $("#lilv").html($(obj).html());
        $("#lilv").attr("data", lt)
    }
    hideMenu();

}

function exc_zuhe(v) {
    document.calc1.type.value = v;
    document.calc1.diakuan_type.value = v;
    //var fmobj = document.calc1;
    var numarr = $(".jsqname").find("div");
    if (v == 3) {
        document.all.calc1_zuhe.style.display = 'block';
        $$("calc1_ctype").style.display = 'none';
        $("#sd_1").attr("class", "td02");
        $("#sd_2").attr("class", "td02");
        $("#sd_3").attr("class", "td01");
    } else {
        document.all.calc1_zuhe.style.display = 'none';
        $$("calc1_ctype").style.display = 'block';
        document.calc1.daikuan_total000.value = "";
        if (v == 1) {
            //$("#divlilv").show();
            $("#sd_1").attr("class", "td01");
            $("#sd_2").attr("class", "td02");
            $("#sd_3").attr("class", "td02");
        }
        else {
            //$("#divlilv").hide();
            $("#sd_1").attr("class", "td02");
            $("#sd_2").attr("class", "td01");
            $("#sd_3").attr("class", "td02");
        }
    }
}

function formReset(fmobj) {
    fmobj.reset();
    if (fmobj.name == "calc1") {
        document.all.calc1_js_div1.style.display = 'block';
        document.all.calc1_js_div2.style.display = 'none';
        document.all.calc1_zuhe.style.display = 'none';
        document.all.calc1_benjin.style.display = 'none';
    } else {
        document.all.calc2_js_div1.style.display = 'block';
        document.all.calc2_js_div2.style.display = 'none';
        document.all.calc2_zuhe.style.display = 'none';
        document.all.calc2_benxi.style.display = 'none';
    }
}

//��ʾ�ұߵıȽ�div
function showRightDiv(fmobj) {
    if (ext_total(fmobj) == false) { return; }
    //alert(document.calc1.month_money2.value);
    var a = window.open('', 'calc_win', 'status=yes,scrollbars=yes,resizable=yes,width=550,height=500,left=0,top=0')//790*520
    if (fmobj.name == "calc1") {
        document.calc1.target = "calc_win";
        document.calc1.submit();
    } else {
        document.calc2.target = "calc_win";
        document.calc2.submit();
    }
}

//��֤�Ƿ�Ϊ����
function reg_Num(str) {
    if (str.length == 0) { return false; }
    var Letters = "1234567890.";

    for (i = 0; i < str.length; i++) {
        var CheckChar = str.charAt(i);
        if (Letters.indexOf(CheckChar) == -1) { return false; }
    }
    return true;
}

//�õ�����
function getlilv(lilv_class, type, years) {
    var lilv_class = parseInt(lilv_class);
    if (years == 1)
    { return lilv_array[lilv_class][type][1]; }
    else
    {
        if (years <= 5) {
            return lilv_array[lilv_class][type][5];
        } else {
            return lilv_array[lilv_class][type][10];
        }
    }
}

//���𻹿���»����(����: ������ / �����ܶ� / �������·� / ���ǰ��0��length-1)
function getMonthMoney2(lilv, total, month, cur_month) {
    var lilv_month = lilv / 12;//������
    //return total * lilv_month * Math.pow(1 + lilv_month, month) / ( Math.pow(1 + lilv_month, month) -1 );
    var benjin_money = total / month;
    return (total - benjin_money * cur_month) * lilv_month + benjin_money;
}

//��Ϣ������»����(����: ������/�����ܶ�/�������·�)
function getMonthMoney1(lilv, total, month) {
    var lilv_month = lilv / 12;//������
    return total * lilv_month * Math.pow(1 + lilv_month, month) / (Math.pow(1 + lilv_month, month) - 1);
}

function chanage_type_mm(n) {
    $$("dengeben").value = n;
    if (n == 1) {
        $("#bj_1").addClass("myon");
        $("#bj_2").removeClass("myon");
    }
    else {
        $("#bj_2").addClass("myon");
        $("#bj_1").removeClass("myon");
    }
}

function ext_total(fmobj) {

    $("#rz_sdlv").html($("#sdlv").val() + "%");
    //var fmobj = document.calc1;
    //������»�����������
    while ((k = fmobj.month_money2.length - 1) >= 0) {
        fmobj.month_money2.options.remove(k);
    }
    var years = $("#years").attr("data");
    var month = $("#years").attr("data") * 12;

    $$("month1").innerHTML = month + " ��";
    fmobj.month2.value = month + "(��)";
    if (fmobj.type.value == 3) {
        $("#canshu_1").hide();
        $("#canshu_2").show();
        $("#zonge").show();
        //--  ����ʹ���(����ʹ���ļ��㣬ֻ����ҵ�����͹����������йأ��Ͱ������ܶ�����޹�)
        if (!reg_Num(fmobj.total_sy.value)) { alert("����ʹ�������д�̴�����"); fmobj.total_sy.focus(); return false; }
        if (!reg_Num(fmobj.total_gjj.value)) { alert("����ʹ�������д���������"); fmobj.total_gjj.focus(); return false; }
        if (fmobj.total_sy.value == null) { fmobj.total_sy.value = 0; }
        if (fmobj.total_gjj.value == null) { fmobj.total_gjj.value = 0; }
        var total_sy = fmobj.total_sy.value;
        var total_gjj = fmobj.total_gjj.value;
        //$$("fangkuan_total1").innerHTML = "��";//�����ܶ�
        fmobj.fangkuan_total2.value = "��";//�����ܶ�
        //$$("money_first1").innerHTML = 0;//���ڸ���
        fmobj.money_first2.value = 0;//���ڸ���

        //�����ܶ�
        var total_sy = parseInt(fmobj.total_sy.value) * 10000;
        var total_gjj = parseInt(fmobj.total_gjj.value) * 10000;
        var daikuan_total = total_sy + total_gjj;

        $$("zh_daikuan_total").innerHTML = parseInt(fmobj.total_sy.value) + "��";
        $$("zh_gjj_total").innerHTML = parseInt(fmobj.total_gjj.value) + "��Ԫ";
        $$("zh_month1").innerHTML = month + " ��";
        fmobj.daikuan_total2.value = daikuan_total;
        $("#zh_rz_gjlv").html($("#gjlv").val() + "%");
        $("#zh_rz_sdlv").html($("#sdlv").val() + "%");
        $("#zh_all_total1").html(daikuan_total);

        //�»���
        var lilv_sd = getlilv($("#lilv").attr("data"), 1, years);//�õ��̴�����
        var lilv_gjj = getlilv($("#lilv").attr("data"), 2, years);//�õ�����������

        //1.���𻹿�
        //�»���
        var all_total2 = 0;
        var month_money2 = "";
        for (j = 0; j < month; j++) {
            //���ú�������: �����»����
            huankuan = getMonthMoney2(lilv_sd, total_sy, month, j) + getMonthMoney2(lilv_gjj, total_gjj, month, j);
            all_total2 += huankuan;
            huankuan = Math.round(huankuan * 100) / 100;
            //fmobj.month_money2.options[j] = new Option( (j + 1) +"��," + huankuan + "(Ԫ)", huankuan);
            month_money2 += "<span style='display:inline-block;width:80px'>" + huankuan + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='color:#b3b5b6;font-size:10px'>��" + (j + 1) + "��</span><br>";
        }
        fmobj.month_money2.value = month_money2;
        //�����ܶ�
        fmobj.all_total2.value = Math.round(all_total2 * 100) / 100;
        //֧����Ϣ��
        fmobj.accrual2.value = Math.round((all_total2 - daikuan_total) * 100) / 100;

        //2.��Ϣ����
        //�¾�����
        var month_money1 = getMonthMoney1(lilv_sd, total_sy, month) + getMonthMoney1(lilv_gjj, total_gjj, month);//���ú�������
        $$("month_money1").innerHTML = Math.round(month_money1 * 100) / 100;
        //�����ܶ�
        var all_total1 = month_money1 * month;
        $$("all_total1").innerHTML = Math.round(all_total1 * 100) / 100;
        //֧����Ϣ��
        $$("accrual1").innerHTML = Math.round((all_total1 - daikuan_total) * 100) / 100;
    } else {
        $("#canshu_1").show();
        $("#canshu_2").hide();
        $("#zonge").hide();



        //--  ��ҵ������������
        var lilv = getlilv($("#lilv").attr("data"), fmobj.type.value, $("#years").attr("data"));//�õ�����
        //------------ ���ݴ����ܶ����
        if (fmobj.daikuan_total000.value.length == 0) { alert("����д�����ܶ�"); fmobj.daikuan_total000.focus(); return false; }

        //�����ܶ�
        //$$("fangkuan_total1").innerHTML = "��";
        fmobj.fangkuan_total2.value = "��";
        //�����ܶ�
        var daikuan_total = fmobj.daikuan_total000.value * 10000;
        $$("daikuan_total1").innerHTML = fmobj.daikuan_total000.value + "��";
        fmobj.daikuan_total2.value = daikuan_total;
        //���ڸ���
        //$$("money_first1").innerHTML = 0;
        fmobj.money_first2.value = 0;
        //1.���𻹿�
        //�»���
        var all_total2 = 0;
        var month_money2 = "";
        for (j = 0; j < month; j++) {
            //���ú�������: �����»����
            huankuan = getMonthMoney2(lilv, daikuan_total, month, j);
            all_total2 += huankuan;
            huankuan = Math.round(huankuan * 100) / 100;
            //fmobj.month_money2.options[j] = new Option( (j + 1) +"��," + huankuan + "(Ԫ)", huankuan);
            month_money2 += "<span style='display:inline-block;width:80px'>" + huankuan + "</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span style='color:#b3b5b6;font-size:10px'>��" + (j + 1) + "��</span><br>";
        }
        fmobj.month_money2.value = month_money2;
        //�����ܶ�
        fmobj.all_total2.value = Math.round(all_total2 * 100) / 100;
        //֧����Ϣ��
        fmobj.accrual2.value = Math.round((all_total2 - daikuan_total) * 100) / 100;
        //2.��Ϣ����
        //�¾�����
        var month_money1 = getMonthMoney1(lilv, daikuan_total, month);//���ú�������
        $$("month_money1").innerHTML = Math.round(month_money1 * 100) / 100;
        //�����ܶ�
        var all_total1 = month_money1 * month;
        $$("all_total1").innerHTML = Math.round(all_total1 * 100) / 100;
        //֧����Ϣ��
        $$("accrual1").innerHTML = Math.round((all_total1 - daikuan_total) * 100) / 100;
    }
    if ($$("dengeben").value == 2) {
        //$$("fangkuan_total1").innerHTML = fmobj.fangkuan_total2.value;
        $$("daikuan_total1").innerHTML = fmobj.daikuan_total2.value;
        $$("all_total1").innerHTML = fmobj.all_total2.value;
        $$("accrual1").innerHTML = fmobj.accrual2.value;
        //$$("money_first1").innerHTML = fmobj.money_first2.value;
        $$("month1").innerHTML = fmobj.month2.value;
        $("#benxi_txt").html("�ȶ��");
        $("#zh_benxi_txt").html("�ȶ��");
        $$("month_money1").innerHTML = fmobj.month_money2.value;

    }
    else {
        $("#benxi_txt").html("�ȶϢ");
        $("#zh_benxi_txt").html("�ȶϢ");
    }
    mytotal()
}

//��ǰ���L����
function play() {
    if (document.tqhdjsq.dkzws.value == '') {
        alert('����������ܶ�');
        return;
    } else dkzys = parseFloat(document.tqhdjsq.dkzws.value) * 10000;
    if (document.tqhdjsq.tqhkfs[1].checked && document.tqhdjsq.tqhkws.value == '') {
        alert('�����벿����ǰ������');
        return;
    }
    s_yhkqs = parseInt(document.tqhdjsq.yhkqs.value);

    //������
    if ($$("tqhklx").value == 1) {
        if (s_yhkqs > 60) {
            dklv = getlilv(document.tqhdjsq.dklv_class.value, 2, 10) / 12; //�������������5������4.23%
        } else {
            dklv = getlilv(document.tqhdjsq.dklv_class.value, 2, 3) / 12;  //�������������5��(��)����3.78%
        }
    }
    if ($$("tqhklx").value == 0) {
        if (s_yhkqs > 60) {
            dklv = getlilv(document.tqhdjsq.dklv_class.value, 1, 10) / 12; //��ҵ�Դ�������5������5.31%
        } else {
            dklv = getlilv(document.tqhdjsq.dklv_class.value, 1, 3) / 12; //��ҵ�Դ�������5��(��)����4.95%
        }
    }

    //�ѻ���������
    yhdkqs = (parseInt(document.tqhdjsq.tqhksjn.value) * 12 + parseInt(document.tqhdjsq.tqhksjy.value)) - (parseInt(document.tqhdjsq.yhksjn.value) * 12 + parseInt(document.tqhdjsq.yhksjy.value));

    if (yhdkqs < 0 || yhdkqs > s_yhkqs) {
        alert('Ԥ����ǰ����ʱ�����һ�λ���ʱ����ì�ܣ����ʵ');
        return false;
    }

    yhk = dkzys * (dklv * Math.pow((1 + dklv), s_yhkqs)) / (Math.pow((1 + dklv), s_yhkqs) - 1);
    yhkjssj = Math.floor((parseInt(document.tqhdjsq.yhksjn.value) * 12 + parseInt(document.tqhdjsq.yhksjy.value) + s_yhkqs - 2) / 12) + '��' + ((parseInt(document.tqhdjsq.yhksjn.value) * 12 + parseInt(document.tqhdjsq.yhksjy.value) + s_yhkqs - 2) % 12 + 1) + '��';
    yhdkys = yhk * yhdkqs;

    yhlxs = 0;
    yhbjs = 0;
    for (i = 1; i <= yhdkqs; i++) {
        yhlxs = yhlxs + (dkzys - yhbjs) * dklv;
        yhbjs = yhbjs + yhk - (dkzys - yhbjs) * dklv;
    }

    remark = '';
    if (document.tqhdjsq.tqhkfs[1].checked) {
        tqhkys = parseInt($$("tqhkws").innerHTML) * 10000;
        if (tqhkys + yhk >= (dkzys - yhbjs) * (1 + dklv)) {
            remark = '������ǰ��������㹻������Ƿ���';
        } else {
            yhbjs = yhbjs + yhk;
            byhk = yhk + tqhkys;
            if (document.tqhdjsq.clfs[0].checked) {
                yhbjs_temp = yhbjs + tqhkys;
                for (xdkqs = 0; yhbjs_temp <= dkzys; xdkqs++) yhbjs_temp = yhbjs_temp + yhk - (dkzys - yhbjs_temp) * dklv;
                xdkqs = xdkqs - 1;
                xyhk = (dkzys - yhbjs - tqhkys) * (dklv * Math.pow((1 + dklv), xdkqs)) / (Math.pow((1 + dklv), xdkqs) - 1);
                jslx = yhk * s_yhkqs - yhdkys - byhk - xyhk * xdkqs;
                xdkjssj = Math.floor((parseInt(document.tqhdjsq.tqhksjn.value) * 12 + parseInt(document.tqhdjsq.tqhksjy.value) + xdkqs - 2) / 12) + '��' + ((parseInt(document.tqhdjsq.tqhksjn.value) * 12 + parseInt(document.tqhdjsq.tqhksjy.value) + xdkqs - 2) % 12 + 1) + '��';
            } else {
                xyhk = (dkzys - yhbjs - tqhkys) * (dklv * Math.pow((1 + dklv), (s_yhkqs - yhdkqs))) / (Math.pow((1 + dklv), (s_yhkqs - yhdkqs)) - 1);
                jslx = yhk * s_yhkqs - yhdkys - byhk - xyhk * (s_yhkqs - yhdkqs);
                xdkjssj = yhkjssj;
            }
        }
    }

    if (document.tqhdjsq.tqhkfs[0].checked || remark != '') {
        byhk = (dkzys - yhbjs) * (1 + dklv);
        xyhk = 0;
        jslx = yhk * s_yhkqs - yhdkys - byhk;
        xdkjssj = document.tqhdjsq.tqhksjn.value + '��' + document.tqhdjsq.tqhksjy.value + '��';
    }

    $$("ykhke").innerHTML = Math.round(yhk * 100) / 100;
    $$("yhkze").innerHTML = Math.round(yhdkys * 100) / 100;
    $$("yhlxe").innerHTML = Math.round(yhlxs * 100) / 100;
    $$("gyyihke").innerHTML = Math.round(byhk * 100) / 100;
    $$("xyqyhke").innerHTML = Math.round(xyhk * 100) / 100;
    $$("jslxzc").innerHTML = Math.round(jslx * 100) / 100;
    $$("yzhhkq").innerHTML = yhkjssj;
    $$("xdzhhkq").innerHTML = xdkjssj;
    $$("jsjgts").innerHTML = remark;
    mytotal();
}

function runjs3(obj) {
    dj3 = parseFloat(obj.dj3.value);
    mj3 = parseFloat(obj.mj3.value);
    fkz3 = dj3 * mj3;
    yh = fkz3 * 0.0005;
    if (dj3 <= 9432) q = fkz3 * 0.015;
    else if (dj3 > 9432) q = fkz3 * 0.03;
    if (mj3 <= 120) fw = 500;
    else if (120 < mj3 <= 5000) fw = 1500;
    if (mj3 > 5000) fw = 5000;
    gzh = fkz3 * 0.003;
    $$("yh").innerHTML = Math.round(yh * 100, 5) / 100;
    $$("fkz3").innerHTML = Math.round(fkz3 * 100, 5) / 100;
    $$("q").innerHTML = Math.round(q * 100, 5) / 100;
    $$("gzh").innerHTML = Math.round(gzh * 100, 5) / 100;
    $$("wt").innerHTML = Math.round(gzh * 100, 5) / 100;
    $$("fw").innerHTML = Math.round(fw * 100, 5) / 100;
    mytotal();
}

//���˹�����
var l1_5 = 0.0405;
var l6_30 = 0.0459;

function isIdCardNo(sNo) {
    sNo = sNo.toString();
    if (sNo.length == 18) {
        var a, b, c;
        if (!isInt(sNo.substr(0, 17))) { return false; }

        a = parseInt(sNo.substr(0, 1)) * 7 + parseInt(sNo.substr(1, 1)) * 9 + parseInt(sNo.substr(2, 1)) * 10;
        a = a + parseInt(sNo.substr(3, 1)) * 5 + parseInt(sNo.substr(4, 1)) * 8 + parseInt(sNo.substr(5, 1)) * 4;
        a = a + parseInt(sNo.substr(6, 1)) * 2 + parseInt(sNo.substr(7, 1)) * 1 + parseInt(sNo.substr(8, 1)) * 6;
        a = a + parseInt(sNo.substr(9, 1)) * 3 + parseInt(sNo.substr(10, 1)) * 7 + parseInt(sNo.substr(11, 1)) * 9;
        a = a + parseInt(sNo.substr(12, 1)) * 10 + parseInt(sNo.substr(13, 1)) * 5 + parseInt(sNo.substr(14, 1)) * 8;
        a = a + parseInt(sNo.substr(15, 1)) * 4 + parseInt(sNo.substr(16, 1)) * 2;
        b = a % 11;

        if (b == 2) {
            c = sNo.substr(17, 1);
            //c = sNo.substr(17, 1).toUpperCase();
        } else {
            c = parseInt(sNo.substr(17, 1));
        }

        switch (b) {
            case 0: if (c != 1) { return false; } break;
            case 1: if (c != 0) { return false; } break;
            case 2: if (c != "X") { return false; } break;
            case 3: if (c != 9) { return false; } break;
            case 4: if (c != 8) { return false; } break;
            case 5: if (c != 7) { return false; } break;
            case 6: if (c != 6) { return false; } break;
            case 7: if (c != 5) { return false; } break;
            case 8: if (c != 4) { return false; } break;
            case 9: if (c != 3) { return false; } break;
            case 10: if (c != 2) { return false; }
        }
    } else {
        if (!isInt(sNo)) { return false; }
    }

    switch (sNo.length) {
        case 15: if (isValidDate(sNo.substr(6, 2), sNo.substr(8, 2), sNo.substr(10, 2))) { return true; }
        case 18: if (isValidDate(sNo.substr(6, 4), sNo.substr(10, 2), sNo.substr(12, 2))) { return true; }
    }
    return false;
}

function isValidDate(iY, iM, iD) {
    var undefined;
    if (iY != undefined && !isNaN(iY) && iY >= 0 && iY <= 9999 && iM != undefined && !isNaN(iM) && iM >= 1 && iM <= 12 && iD != undefined && !isNaN(iD) && iD >= 1 && iD <= 31) {
        if (iY < 50) iY = 2000 + iY;
        else if (iY < 100) iY = 1900 + iY;
        if (iM == 2 && (isLeapYear(iY) && iD > 29 || !isLeapYear(iY) && iD > 28) || iD == 31 && (iM < 7 && iM % 2 == 0 || iM > 7 && iM % 2 == 1))
            return false;
        else return true;
    } else
        return false;
}

function isLeapYear(year) {
    if ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0)) {
        return true;
    }
    return false;
}

function isEmpty(str) {
    if ((str == null) || (str.length == 0)) return true;
    else return (false);
}

function isInt(theStr) {
    var flag = true;

    if (isEmpty(theStr)) { flag = false; }
    else {
        for (var i = 0; i < theStr.length; i++) {
            if (theStr.substring(i, i + 1) == ".") {
                flag = false;
                break;
            }
        }
    }
    return (flag);
}

function isnumeric(p) {
    if (p == "")
        return false;
    var l = p.length;
    var count = 0;
    for (var i = 0; i < l; i++) {
        var digit = p.charAt(i);
        if (digit == ".") {
            ++count;
            if (count > 1) return false;
        }
        else if (digit < "0" || digit > "9")
            return false;
    }
    return true;
}
function Format(num, dotLen) {

    var dot = 0
    var num1 = 0
    //change by liyugen
    if (typeof dotLen == "undefined")
        dot = 2
    else
        dot = dotLen
    if (isNaN(parseFloat(num)))
        return 0
    else
        num1 = parseFloat(num)
    var n1 = Math.pow(10, dot)
    if (n1 == 0)
        var iValue = Math.round(num1)
    else
        var iValue = Math.round(num1 * n1) / n1
    var sValue = iValue.toString();
    if (sValue.indexOf(".") == -1) {
        sValue = sValue + ".00";
    }
    else {
        if (sValue.indexOf(".") == sValue.length - 1) {
            sValue = sValue + "00";
        }
        else if (sValue.indexOf(".") == sValue.length - 2) {
            sValue = sValue + "0";
        }
    }
    return sValue
}
function c_id_card(obj) {
    var id_card = obj.id_card.value;//���֤
    var age = 0;
    age_qx.innerText = '';
    if (id_card.length > 0) {
        if (!isInt(id_card)) { alert('���֤�������������'); return false; }
        if (id_card.length != 15 && id_card.length != 18) { alert('���֤���볤��Ϊ15��18λ'); return false; }
        if (!isIdCardNo(id_card)) { alert('��������ȷ�����֤����'); return false; }
        var a = new Date();
        var y = Number(a.getFullYear());
        if (id_card.length == 15) { age = y - Number('19' + id_card.substr(6, 2)); } else { age = y - Number(id_card.substr(6, 4)); }
        var max_qx = 70 - age; if (max_qx > 30) { max_qx = 30; }
        age_qx.innerText = '�����' + max_qx + '��';
    }
}
function gjjloan1(obj) {
    var gryjce;//ס������������½ɴ��
    var poyjce;//��żס������������½ɴ��
    var grjcbl;//�ɴ����
    var pojcbl;//��ż�ɴ����
    var xy;//��������
    var fwzj;//�����ܼ�
    var fwxz;//��������
    var dknx;//������������
    var syhk;//���»����

    var dked;//��Ҫ������
    var hkfs;//���ʽ
    var bxhj;//��Ϣ�ϼ�
    var bxhj2;//��Ϣ�ϼƵȱ���

    //�м����
    var gbl;
    var jtysr;//��ͥ������
    var r;//�»���
    var gjjdka;//ס�������������a
    var gjjdkb;//ס�������������b
    var gjjdkc;//ס�������������c

    gryjce = obj.mount2.value;
    if (gryjce <= 0) {
        alert('ס������������½ɴ���Ϊ��,������');
        obj.mount2.value = ''; obj.mount2.focus(); return;
    }

    poyjce = obj.mount3.value;
    if (poyjce.length > 0 && !isnumeric(poyjce))
    { alert("��ż�½ɴ��¼�벻��ȷ"); return; }

    if (obj.jcbl.value == "" || !isInt(obj.jcbl.value) || Number(obj.jcbl.value) <= 0 || Number(obj.jcbl.value) >= 100) {
        alert("�ɴ��������ȷ"); return;
    }
    if (poyjce.length > 0 && (obj.p_bl.value == "" || !isInt(obj.p_bl.value) || Number(obj.p_bl.value) <= 0 || Number(obj.p_bl.value) >= 100)) {
        alert("��ż�ɴ��������ȷ"); return;
    }
    grjcbl = obj.jcbl.value / 100;
    pojcbl = obj.p_bl.value / 100;
    /*
    if (obj.xz[0].checked==true){fwxz=0.9;}
    else {fwxz=0.95;}
    */
    if (obj.xz[0].checked == true) { fwxz = 0.9; }
    else { fwxz = 0.8; }

    if (obj.xy[0].checked == true) { xy = 1.3; }
    else if (obj.xy[1].checked == true) { xy = 1.15; }
    else { xy = 1; }

    fwzj = obj.mount.value;

    if (fwzj <= 0) {
        alert('������������ֵ��ʵ�ʹ��������Ϊ��,������');
        obj.mount.value = ''; return;
    }

    dknx = Math.round(obj.mount10.value);

    if (dknx <= 0) {
        alert('�����������޲���Ϊ��,������');
        obj.mount10.value = ''; return;
    }
    if (dknx > 30) {
        alert('�����������޲��ܴ���30��,����������');
        obj.mount10.value = ''; return;
    }
    var bcv = 0;
    if (dknx > 5) {
        bcv = Math.round(1000000 * l6_30 / 12) / 1000000;
    } else {
        bcv = Math.round(1000000 * l1_5 / 12) / 1000000;
    }
    r = adv_format((10000 * bcv * Math.pow(1 + bcv, 12 * dknx)) / (Math.pow(1 + bcv, 12 * dknx) - 1), 2);


    if (poyjce.length > 0) {
        jtysr = Math.ceil((gryjce / grjcbl + poyjce / pojcbl) * 10) / 10;
    }
    else {
        jtysr = Math.ceil((gryjce / grjcbl) * 10) / 10;
    }
    if (jtysr <= 400) {
        alert('��ͥ���������400�������ϴ�������');
        return;
    }

    gjjdka = Math.min(Math.round((jtysr - 400) / r * 10000 * 10) / 10, 600000);
    gjjdkb = Math.round(gjjdka * xy * 10) / 10;
    gjjdkc = Math.round(fwzj * fwxz * 10) / 10;
    //obj.ze2.value=gjjdka; //jtysr;
    obj.ze2.value = Math.floor(Math.min(gjjdkb, gjjdkc) / 10000 * 10) / 10;
    zgdk = obj.ze2.value; //��ߴ�����













    //�����������ߴ�����

    /*
    ˵��
    ��ͥ�����룽ס������������½ɴ��½ɴ����+��żס������������½ɴ��½ɴ����
    
    ס�������������a������ͥ�����룭400���µȶ�����»����ÿ��Ԫ�»����Ҳ�����40��Ԫ
    
    ���ڸ������õȼ�ΪAAA�ģ�ס�������������b��ס�������������a��130��
    
    ���ڸ������õȼ�ΪAA�ģ�ס�������������b��ס�������������a��115��
    
    ���ڸ������õȼ������ģ�ס�������������b��ס�������������a
    
    �Է�������Ϊ��Ʒ���ڷ��ģ�ס�������������c�������ܼۡ�90��
    
    �Է�������Ϊ�����ģ�ס�������������c�������ܼۡ�95��
    
    ��ߴ�����d��min��b��c��
    
    �ȶ�������ʽ��
    
    
    �ȶ�𻹿ʽ
    
    ���»����=P/��n��12��+����ܶ��I
    
    ���У�PΪ�����IΪ�����ʣ�nΪ�������ޡ�
    
    
      */
    mytotal()
}
function adv_format(value, num)   //��������
{
    var a_str = formatnumber(value, num);
    var a_int = parseFloat(a_str);
    if (value.toString().length > a_str.length) {
        var b_str = value.toString().substring(a_str.length, a_str.length + 1)
        var b_int = parseFloat(b_str);
        if (b_int < 5) {
            return a_str
        }
        else {
            var bonus_str, bonus_int;
            if (num == 0) {
                bonus_int = 1;
            }
            else {
                bonus_str = "0."
                for (var i = 1; i < num; i++)
                    bonus_str += "0";
                bonus_str += "1";
                bonus_int = parseFloat(bonus_str);
            }
            a_str = formatnumber(a_int + bonus_int, num)
        }
    }
    return a_str
}
function formatnumber(value, num)    //ֱ��ȥβ
{
    var a, b, c, i
    a = value.toString();
    b = a.indexOf('.');
    c = a.length;
    if (num == 0) {
        if (b != -1)
            a = a.substring(0, b);
    }
    else {
        if (b == -1) {
            a = a + ".";
            for (i = 1; i <= num; i++)
                a = a + "0";
        }
        else {
            a = a.substring(0, b + num + 1);
            for (i = c; i <= b + num; i++)
                a = a + "0";
        }
    }
    return a
}
function gjjloan2(obj) {

    var gryjce;//ס������������½ɴ��
    var poyjce;//��żס������������½ɴ��
    var grjcbl;//�ɴ����
    var pojcbl;//��ż�ɴ����
    var xy; //��������
    var fwzj;//�����ܼ�
    var fwxz;//��������
    var dknx;//������������
    var syhk; //���»����

    var dked;//��Ҫ������
    var hkfs;//���ʽ
    var bxhj; //��Ϣ�ϼ�
    var bxhj2; //��Ϣ�ϼƵȱ���

    //�м����
    var gbl;
    var jtysr; //��ͥ������
    var r;//�»���
    var rb;
    var gjjdka;//ס�������������a
    var gjjdkb;//ס�������������b
    var gjjdkc;//ס�������������c


    gryjce = obj.mount2.value;
    if (gryjce <= 0) {
        alert('ס������������½ɴ���Ϊ��,������.');
        obj.mount2.value = '';
        obj.mount.focus();
        return;
    }

    poyjce = obj.mount3.value;
    /*if (obj.jcbl[0].checked==true)
    {grjcbl=0.08;}
    else {grjcbl=0.1;}
    
    if (obj.p_bl[0].checked==true){pojcbl=0.08;}
    else {pojcbl=0.1;}
    */
    grjcbl = obj.jcbl.value / 100;
    pojcbl = obj.p_bl.value / 100;
    if (obj.xz[0].checked == true) { fwxz = 0.9; }
    else { fwxz = 0.8; }

    if (obj.xy[0].checked == true) { xy = 1.15; }
    else if (obj.xy[1].checked == true) { xy = 1.3; }
    else { xy = 1; }



    fwzj = obj.mount.value;

    if (fwzj <= 0) {
        alert('�����ܼ۲���Ϊ��,������');
        obj.mount.value = ''; return;
    }

    dknx = Math.round(obj.mount10.value);
    //alert(dknx);
    if (dknx <= 0) {
        alert('�����������޲���Ϊ��,������');
        obj.mount10.value = ''; return;
    }





    var bcv = 0;
    if (dknx > 5) {
        bcv = Math.round(1000000 * l6_30 / 12) / 1000000;
    } else {
        bcv = Math.round(1000000 * l1_5 / 12) / 1000000;
    }
    r = adv_format((10000 * bcv * Math.pow(1 + bcv, 12 * dknx)) / (Math.pow(1 + bcv, 12 * dknx) - 1), 2);

    jtysr = Math.ceil((gryjce / grjcbl + poyjce / pojcbl) * 10) / 10;
    gjjdka = Math.min(Math.round((jtysr - 400) / r * 10000 * 10) / 10, 600000);
    gjjdkb = Math.round(gjjdka * xy * 10) / 10;
    gjjdkc = Math.round(fwzj * fwxz * 10) / 10;
    //obj.ze2.value=gjjdka; //jtysr;
    //obj.ze2.value=Math.floor(Math.min(gjjdkb,gjjdkc)/10000*10)/10;

    //����2
    zgdk = obj.ze2.value; //��ߴ�����

    dked = Math.round(obj.need.value * 10) / 10;

    obj.need.value = dked;

    if (dked == 0) {
        alert('��Ҫ�Ĵ����Ȳ���Ϊ��,������');
        obj.need.value = ''; return;
    }
    if (dked < 0) {
        alert('����Ĵ����Ȳ�����Ҫ��,������');
        obj.need.value = ''; return;
    }


    if (dked > zgdk) {
        alert('���ܸ�����ߴ�����,����������');
        obj.need.value = ''; return;
    }


    hkfs = obj.select.value;

    if (hkfs == 1) {
        //obj.ze22.value=Math.ceil(dked*r*100)/100;
        //==============================modify by xujianfei
        var ylv_new;

        if (dknx >= 1 && dknx <= 5)
            ylv_new = l1_5 / 12;
        else
            ylv_new = l6_30 / 12;


        var ncm = parseFloat(ylv_new) + 1;//n����

        var dknx_new = dknx * 12;



        total_ncm = Math.pow(ncm, dknx_new)

        $$("ze22").innerHTML = Math.round(((dked * 10000 * ylv_new * total_ncm) / (total_ncm - 1)) * 100) / 100;
        var pp = Math.round(((dked * 10000 * ylv_new * total_ncm) / (total_ncm - 1)) * 100) / 100;

        //=========================================================
        gbl = Math.round(Math.ceil(dked * r * 100) / 100 / jtysr * 1000) / 10;
        obj.yj2.value = gbl;
        //bxhj=Math.round(r*dked*dknx*12*100)/100;
        bxhj = Math.round(pp * dknx * 12 * 100) / 100;
        $$("lx2").innerHTML = bxhj;
        $$("f1").style.display = "";
        $$("f2").style.display = "none";
        $$("f3").style.display = "none";
        $$("fangshi1").innerHTML = "�ȶϢ"
    }
    if (hkfs == 2) {
        if (dknx > 5) {
            rb = l6_30 * 100;
        } else {
            rb = l1_5 * 100;
        }

        syhk = Math.round((dked * 10000 / (dknx * 12) + dked * 10000 * rb / (100 * 12)) * 100) / 100;
        $$("sfk2").innerHTML = syhk;
        var yhke; //�»����
        var bxhj; //��Ϣ�ϼ�
        var dkys; //��������
        var sydkze;//��ǰʣ������ܶ�
        var yhkbj; //�»����
        dkys = dknx * 12;
        yhkbj = dked * 10000 / dkys;

        yhke = syhk;
        sydkze = dked * 10000 - yhkbj;
        bxhj = syhk;
        for (var count = 2; count <= dkys; ++count) {
            //yhke=Math.round((dked*10000/(dknx*12)+sydkze*rb/(100*12))*100)/100;
            yhke = dked * 10000 / dkys + sydkze * rb / 1200;
            sydkze -= yhkbj;
            bxhj += yhke;
        }
        $$("lx3").innerHTML = Math.round(bxhj * 100) / 100;
        $$("f1").style.display = "none";
        $$("f2").style.display = "";
        $$("f3").style.display = "none";
        $$("fangshi2").innerHTML = "�ȶ��"
    }

    if (hkfs == 3) {

        switch (dknx) {//���ɻ���ʽ����ͻ������ձ�,�������ʲ��޸�
            case 1: rb = 83.04 / 100; break;
            case 2: rb = 81.08 / 100; break;
            case 3: rb = 79.12 / 100; break;
            case 4: rb = 77.16 / 100; break;
            case 5: rb = 75.20 / 100; break;
            case 6: rb = 73.24 / 100; break;
            case 7: rb = 71.28 / 100; break;
            case 8: rb = 69.32 / 100; break;
            case 9: rb = 67.36 / 100; break;
            case 10: rb = 65.40 / 100; break;
            case 11: rb = 63.44 / 100; break;
            case 12: rb = 61.48 / 100; break;
            case 13: rb = 59.52 / 100; break;
            case 14: rb = 57.56 / 100; break;
            case 15: rb = 55.60 / 100; break;
            case 16: rb = 53.64 / 100; break;
            case 17: rb = 51.68 / 100; break;
            case 18: rb = 49.72 / 100; break;
            case 19: rb = 47.76 / 100; break;
            case 20: rb = 45.80 / 100; break;
            case 21: rb = 43.84 / 100; break;
            case 22: rb = 41.88 / 100; break;
            case 23: rb = 39.92 / 100; break;
            case 24: rb = 37.96 / 100; break;
            case 25: rb = 36.00 / 100; break;
            case 26: rb = 34.04 / 100; break;
            case 27: rb = 32.08 / 100; break;
            case 28: rb = 30.12 / 100; break;
            case 29: rb = 28.16 / 100; break;
            case 30: rb = 26.20 / 100; break;
        }
        var yhke; //�»����
        var ll;//����
        var zhbj;//���һ�ڱ���
        zhbj = Math.round(dked * 10000 * rb * 100) / 100;
        if (dknx <= 5) {
            ll = l1_5 / 12;
            zdhkll = 0.0378 / 12;
            var total_gjj = Math.pow(zdhkll + 1, dknx * 12);
            syhk = Math.ceil(dked * 10000 * zdhkll * total_gjj / (total_gjj - 1));
        }
        else {
            ll = l6_30 / 12;
            zdhkll = 0.0423 / 12;
            var total_gjj = Math.pow(zdhkll + 1, dknx * 12 - 1);
            syhk = Math.ceil((dked * 10000 - zhbj) * zdhkll * total_gjj / (total_gjj - 1) + zhbj * zdhkll);
        }
        $$("sfk3").innerHTML = syhk;       //��ͻ����
        var zhyqbj = dked * 10000;
        var zchlx = 0;//�ܳ�����Ϣ
        for (i = 1; i < dknx * 12 ; i++) {
            zchlx += Math.round(zhyqbj * ll * 100) / 100;
            zhyqbj = Math.round((zhyqbj - (syhk - Math.round(zhyqbj * ll * 100) / 100)) * 100) / 100;
        }
        var sydkze = dked * 10000 - syhk;
        $$("lx4").innerHTML = zhyqbj;    //����ڱ���
        $$("lx5").innerHTML = Math.round(zhyqbj * ll * 100) / 100;
        /*for (i=1;i<dknx*12 ;i++ )
        {
            zchlx+=Math.round((dked*10000-(syhk-Math.round(dked*10000*ll*100)/100))*ll*100)/100;
        }*/
        zchlx += Math.round(zhyqbj * ll * 100) / 100;
        $$("lx6").innerHTML = Math.round(zchlx * 100) / 100;
        $$("f1").style.display = "none";
        $$("f2").style.display = "none";
        $$("f3").style.display = "";
        $$("fangshi3").innerHTML = "���ɻ���"
    }


    //�����������ߴ�����
    /*
    ˵��
    ��ͥ�����룽ס������������½ɴ��½ɴ����+��żס������������½ɴ��½ɴ����
    
    ס�������������a������ͥ�����룭400���µȶ�����»����ÿ��Ԫ�»����Ҳ�����40��Ԫ
    
    ���ڸ������õȼ�ΪAAA�ģ�ס�������������b��ס�������������a��130��
    
    ���ڸ������õȼ�ΪAA�ģ�ס�������������b��ס�������������a��115��
    
    ���ڸ������õȼ������ģ�ס�������������b��ס�������������a
    
    �Է�������Ϊ��Ʒ���ڷ��ģ�ס�������������c�������ܼۡ�90��
    
    �Է�������Ϊ�����ģ�ס�������������c�������ܼۡ�95��
    
    ��ߴ�����d��min��b��c��
    
    �ȶ�������ʽ��
    
    �ȶ�𻹿ʽ
    
    ���»����=P/��n��12��+����ܶ��I
    
    ���У�PΪ�����IΪ�����ʣ�nΪ�������ޡ� */
    mytotalsec();
}
function gjjloan3(obj) {
    var dkye = 0;//����Ҫ���������
    var dkzqs = 0;//����������
    var gdhke = 0;//�̶������
    var sxhke = 0;//���軹���
    var sxhky = 0; //����Ҫ�Ļ�������
    var zhhke = 0;//��󻹿��
    var zglx = 0;//�ܹ���Ϣ
    var dylx = 0; //������Ϣ
    var syqs = 0;
    syqs = Number(obj.lx8_sy.value);
    dkye = Number(obj.lx7.value);
    if (dkye <= 0 || dkye > 780000 || isNaN(dkye)) {
        alert("����������벻��ȷ");
        return;
    }
    var ll;   //������
    if (obj.lx8[1].checked)
    { ll = Math.round(1000000 * l6_30 / 12) / 1000000; }
    else
    { ll = Math.round(1000000 * l1_5 / 12) / 1000000; }
    /*if(dkzqs<=0 || dkzqs>360 || isNaN(dkzqs))
    {
        alert("��������������ȷ!");
        return;
    }*/

    gdhke = Number(obj.lx9.value);
    if (Number(syqs) <= 0 || isNaN(syqs)) {
        alert("��������ȷ��ʣ������!");
        return;
    }
    if (Number(gdhke) <= 0 || isNaN(gdhke)) {
        alert("��������ȷ�Ĺ̶������!");
        return;
    }

    var first_lx = Math.round(dkye * ll * 100) / 100;
    if (first_lx > gdhke)
    { alert('�̶������������������Ϣ ' + first_lx); obj.lx9.focus(); obj.lx9.select(); return; }
    var yjqs = 0;//Math.ceil(dkye/gdhke);
    var sxhky = 0;
    for (var i = 1; i < syqs; i++) {
        //��Ҫ������+1
        sxhky = sxhky + 1;
        //����һ���µ���Ϣ
        dylx = Math.round(dkye * ll * 100) / 100;
        //�ۼ���Ϣ
        zglx = zglx + dylx;
        //Math.round ((zglx + dylx) *100) /100 ;

        if (dkye + dylx >= gdhke) {
            if (dkye + dylx == gdhke) zhhke = dkye + dylx;
            dkye = Math.round((dkye - (gdhke - dylx)) * 100) / 100;
            //Math.round(  (dkye - ( gdhke - dylx ))*100  ) /100;
        }
        else {
            zhhke = dkye + dylx;
            dkye = -1;
            break;
        }

    }
    if (dkye > 0) {
        sxhky = sxhky + 1;
        dylx = Math.round(dkye * ll * 100) / 100;
        zglx = zglx + dylx;
        zhhke = dkye + dylx
    }


    if (sxhky > syqs) {
        alert("���벻��ȷ,�����º˶Դ��������¹̶������!" + sxhky);
        return;
    }



    obj.lx10.value = sxhky;
    obj.lx11.value = Format(zhhke, 2);
    obj.lx12.value = Format(zglx, 2);
    //���ʣ�౾��+��Ϣ< �̶�������   ==> �������  ->����ڻ����
}

//������������
rhb = new Array(440.104, 301.103, 231.7, 190.136, 163.753, 144.08, 129.379, 117.991, 108.923, 101.542, 95.425, 90.282, 85.902, 82.133, 78.861, 75.997, 73.473, 71.236, 69.241, 67.455, 65.848, 64.397, 63.082, 61.887, 60.798, 59.802, 58.890, 58.052, 57.282)
yhz = new Array(1.978, 2.9344, 3.8699, 4.7847, 5.6794, 6.5544, 7.4102, 8.2472, 9.0657, 9.8662, 10.6491, 11.4148, 12.1636, 12.8959, 13.6121, 14.3126, 14.9977, 15.6677, 16.3229, 16.9637, 17.5904, 18.2034, 18.8028, 19.389, 19.9624, 20.5231, 21.0715, 21.6078, 22.1323)
function chk01() {
    if (parseFloat(document.myform.rg01.value) < 4.7)
        alert("--��ȷ����" + parseFloat(document.myform.rg01.value) + "��Ԫ?--" + "\n\n" + "��ô��Ŀǰ�в��߱�����������" + "\n\n" + "������ܻ�����ܳＯ������ʽ�")
    if (parseFloat(document.myform.rg01.value) > 10000)
        alert("��ȷ��ӵ�г���һ��Ԫ�Ĺ����ʽ�");

}
function chk02() {
    if (parseFloat(document.myform.rg03.value) > parseFloat(document.myform.rg02.value) * 0.7) {
        alert("��Ԥ�Ƽ�ͥÿ�¿����ڹ���֧���ѳ�����ͥ�������70%��" + "\n\n" + "�Ƿ�ȷ������Ӱ�����������������ѣ�" + "\n\n" + "������40%��" + parseFloat(document.myform.rg02.value) * 0.4 + "Ԫ������")
    }
}
function chk03() {
    if (document.myform.rg01.value == "")
        alert("����д�ֿ����ڹ������ʽ�")
    else
        if (document.myform.rg02.value == "")
            alert("����д�ּ�ͥ������")
        else
            if (document.myform.rg03.value == "")
                alert("����дԤ�Ƽ�ͥÿ�¿����ڹ���֧��")
            else
                if (document.myform.rg06.value == "")
                    alert("����д���ƻ������ݵ����")
                else
                    chk04()

}
function chk04() {
    //���ɹ���ķ����ܼ�=����ͥ������-��ͥ�¹̶�֧������( (��1�������ʣ��޻�������)��1  )�£������ʡ���1�������ʣ��޻���������+�����ʽ� 
    var month = parseInt(document.myform.rg04.options[document.myform.rg04.selectedIndex].value);
    var year = parseInt(month / 12);
    var lilu = 0.00576;
    if (year > 5)
        lilu = 0.00594;
    js00 = parseFloat(document.myform.rg01.value);//�ֳ���
    js01 = parseFloat(document.myform.rg02.value);//������
    js02 = parseFloat(document.myform.rg03.value);//��֧��
    js03 = parseFloat(document.myform.rg06.value);//���

    var d1 = js01 - js02;
    var d2 = Math.pow(1 + lilu, month) - 1;
    var d3 = lilu * Math.pow(1 + lilu, month)

    $$("rg07").innerHTML = Math.round(((d1 * d2) / d3) + js00);
    $$("rg08").innerHTML = parseFloat($$("rg07").innerHTML) / js03;
    mytotal();
    pl();
}

function pl() {
    var XmlHttp;
    XmlHttp = new ActiveXObject("Msxml2.XMLHTTP.3.0");
    XmlHttp.open("POST", "/newhouse_calculateTool_projList.php?s=" + Math.random() + "&city=" + encodeURIComponent($$('abcity').value) + "&price=" + Math.round($$("rg08").innerHTML), false);
    XmlHttp.send(null);
    if (XmlHttp.status == 200) {
        $$('projList').innerHTML = XmlHttp.responseText;
    }
}
function closeCityList() {
    if ($$('searchExpert2').style.display != "none") $$('searchExpert2').style.display = 'none';
}
function ChangeCity(city) { $$('abcity').value = city; $$('showcity').innerHTML = city; closeCityList() }

function showMenu(n) {
    if (n == 2) {
        $("#p_1").hide();
        $("#p_2").show();
        $("#p_3").hide();
    }
    else {
        $("#p_1").hide();
        $("#p_2").hide();
        $("#p_3").show();
    }
}
function hideMenu() {
    $("#p_1").show();
    $("#p_2").hide();
    $("#p_3").hide();
    $$('myresult').style.display = 'none';;
}