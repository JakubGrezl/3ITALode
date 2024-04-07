﻿
using System.Numerics;
using System.Linq;
using Microsoft.VisualBasic.Logging;
namespace _3ITALode
{
    public partial class Form1 : Form
    {
        Hrac hrac1;
        Hrac hrac2;

        Hrac aktualniHrac;

        public Hrac AktualniHrac
        {
            get => aktualniHrac;
            set
            {
                aktualniHrac = value;
            }
        }

        private Policko _nakliknutePolicko;
        private Policko nakliknutePolicko
        {
            get => _nakliknutePolicko;
            set
            {
                if (_nakliknutePolicko != null && _nakliknutePolicko.BackColor == Color.Red)
                    _nakliknutePolicko.BackColor = Color.Turquoise;

                _nakliknutePolicko = value;
                if (_nakliknutePolicko != null)
                    _nakliknutePolicko.BackColor = Color.Red;
            }
        }
        static List<int> lodeNaStavbu = new List<int>()
        {
            5,
            4,
            3,
            3,
            2
        };

        int indexAktualniLode = 0;

        public Form1()
        {
            InitializeComponent();

            VytvorHru();
        }

        private void VytvorHru()
        {
            //Vytvoøení hráèù
            hrac1 = new Hrac("TondaFimlar", label1);
            hrac1.ActivePlayer();
            hrac2 = new Hrac("RandomHovna", label2);

            AktualniHrac = hrac1;
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Policko polickoHrace1 = new Policko(j, i, null, hrac1);
                    polickoHrace1.OnPolickoKliknuto += OnPolickoKliknuto;
                    polickoHrace1.OnPolickoHover += OnPolickoHover;
                    polickoHrace1.OnPolickoLeave += OnPolickoLeave;
                    hrac1.HerniPole[i, j] = polickoHrace1;
                    flowLayoutPanel1.Controls.Add(
                        polickoHrace1
                        );


                    Policko polickoHrace2 = new Policko(j, i, null, hrac2);
                    polickoHrace2.OnPolickoHover += OnPolickoHover;
                    polickoHrace2.OnPolickoLeave += OnPolickoLeave;


                    hrac2.HerniPole[i, j] = polickoHrace2;
                    flowLayoutPanel2.Controls.Add(
                        polickoHrace2
                       );
                }
            }
        }

        private void OnPolickoLeave(Policko policko)
        {
            if (nakliknutePolicko != null)
            {
                int smerX = policko.X - nakliknutePolicko.X;
                int smerY = policko.Y - nakliknutePolicko.Y;


                Point zacatekLode = new Point(nakliknutePolicko.X, nakliknutePolicko.Y);

                var pole = AktualniHrac.HerniPole;

                int aktualniLod = lodeNaStavbu[indexAktualniLode];


                for (int i = 0; i < aktualniLod; i++)
                {
                    AktualniHrac.HerniPole[zacatekLode.Y, zacatekLode.X].RemoveGhost();
                    zacatekLode.Offset(smerX, smerY);
                }
            }
            

        }

        private void OnPolickoHover(Policko policko)
        {

            if (nakliknutePolicko != null)
            {
                int smerX = policko.X - nakliknutePolicko.X;
                int smerY = policko.Y - nakliknutePolicko.Y;


                Point zacatekLode = new Point(nakliknutePolicko.X, nakliknutePolicko.Y);
                if (zacatekLode.X < 0 ||
                    zacatekLode.X >= AktualniHrac.HerniPole.GetLength(1) ||
                    zacatekLode.Y < 0 ||
                    zacatekLode.Y >= AktualniHrac.HerniPole.GetLength(0))
                {
                    // AktualniHrac.HerniPole[zacatekLode.Y, zacatekLode.X].RemoveGhost();
                    return;
                } else
                {
                int aktualniLod = lodeNaStavbu[indexAktualniLode];


                for (int i = 0; i < aktualniLod; i++)
                {
                    AktualniHrac.HerniPole[zacatekLode.Y, zacatekLode.X].Ghost();
                    zacatekLode.Offset(smerX, smerY);
                }

                }


            }
                

        }

        private void OnPolickoKliknuto(Policko policko)
        {
            //Podmínky stavby / Podmínky støelby
            if (AktualniHrac != policko.Hrac || policko.Lod != null)
                return;

            //Kontrola, že na políčku neni loď

            if (nakliknutePolicko == null)
            {
                nakliknutePolicko = policko;
                return;
            }
            //Nesmím kliknout na stejné políèko => jinak odklikávám
            if (nakliknutePolicko == policko)
            {
                nakliknutePolicko = null;
                return;
            }
            //Vezmu obì políèka zjistím smìr
            int smerX = policko.X - nakliknutePolicko.X;
            int smerY = policko.Y - nakliknutePolicko.Y;
            if (Math.Abs(smerX) + Math.Abs(smerY) > 1)
            {
                MessageBox.Show("AAAAAAAAAAAAAAAAAAAAAAAAAA");
                return;
            }

            Point zacatekLode = new Point(nakliknutePolicko.X, nakliknutePolicko.Y);

            int aktualniLod = lodeNaStavbu[indexAktualniLode];

            for (int i = 0; i < aktualniLod; i++)
            {
                //Zjistím jestli se loï vejde
                if (zacatekLode.X < 0 ||
                    zacatekLode.X >= AktualniHrac.HerniPole.GetLength(1) ||
                    zacatekLode.Y < 0 ||
                    zacatekLode.Y >= AktualniHrac.HerniPole.GetLength(0))
                {
                    MessageBox.Show(" IT WONT FIT ");
                    return;
                }

                //Zjistím jestli se nenachází v cestì jiná loï
                if (AktualniHrac.HerniPole[zacatekLode.Y, zacatekLode.X].Lod != null)
                {
                    MessageBox.Show("JE TAM LOĎ");
                    return;
                }
                zacatekLode.Offset(smerX, smerY);
                //     MessageBox.Show(zacatekLode.ToString());
            }


            //Položím loï
            zacatekLode = new Point(nakliknutePolicko.X, nakliknutePolicko.Y);
            Lod lod = new Lod(aktualniLod, new Vector2(smerX, smerY), zacatekLode.X, zacatekLode.Y, 0);
            for (int i = 0; i < aktualniLod; i++)
            {
                AktualniHrac.HerniPole[zacatekLode.Y, zacatekLode.X].Lod = lod;
                zacatekLode.Offset(smerX, smerY);
            }

            indexAktualniLode++;
            nakliknutePolicko = null;

            if (indexAktualniLode >= lodeNaStavbu.Count)
            {
                foreach (Policko polecko in AktualniHrac.HerniPole)
                {
                    polecko.SchovejLod();
                }
                indexAktualniLode = 0;


                if (hrac1.JeReadyNaBitvu && hrac2.JeReadyNaBitvu)
                {
                    MessageBox.Show("Jsou ready");
                    SpustBitvu();
                }
                PrepniHrace();
            }


            //Podmínky jestli políèko mùže být nakliknutelný




            //Støelba => Zmìna hráèù po konci kola
            //Kontrola sestøelených lodí => Checkování konce hry

        }

        private void SpustBitvu()
        {
            throw new NotImplementedException();
        }

        private void PrepniHrace()
        {
            AktualniHrac.DeactivePlayer();
            AktualniHrac = AktualniHrac == hrac1 ? hrac2 : hrac1;
            AktualniHrac.ActivePlayer();
        }
    }
}
