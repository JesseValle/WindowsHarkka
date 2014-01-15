using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Harjoitustyo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            loadData();
        }

        void loadData()
        {
            comboBox4.Items.Clear();
            comboBox1.Items.Clear();
            comboBox3.Items.Clear();
            comboBox2.Items.Clear();

            using (var db = new Luokat.PuhelinluetteloContext())
            {
                var query = from b in db.Postitoimipaikat
                            select b;

                foreach (var a in query)
                {
                    comboBox4.Items.Add(a.PostiNumero);
                    comboBox1.Items.Add(a.PostiNumero);
                }

                var query2 = from c in db.Operaattorit
                             select c;

                foreach (var a in query2)
                {
                    comboBox3.Items.Add(a.OperaattoriNimi);
                    comboBox2.Items.Add(a.OperaattoriNimi);
                }

                var query3 = from a in db.Henkilot
                             from b in db.Postitoimipaikat
                             from c in db.Operaattorit
                             where a.OperaattoriId == c.Id && a.PostiToimipaikkaId == b.Id
                             select new { a.HenkiloId, a.Nimi, a.LahiOsoite, b.PostiNumero, b.PostiToimipaikanNimi, a.PuhNo, c.OperaattoriNimi };

                dataGridView1.DataSource = query3.ToList();
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            using (var db = new Luokat.PuhelinluetteloContext())
            {
                var blog = new Luokat.Henkilo { LahiOsoite = "asd", Nimi = "asd", OperaattoriId = 1, PostiToimipaikkaId = 1, PuhNo = "1231231234" };

                var jep = new Luokat.Operaattori { OperaattoriNimi = "DNA" };

                var das = new Luokat.PostiToimipaikka { PostiNumero = "70500", PostiToimipaikanNimi = "Kuopio" };


                db.Operaattorit.Add(jep);
                db.Postitoimipaikat.Add(das);

                db.SaveChanges();
                db.Henkilot.Add(blog);
                db.SaveChanges();
            }
             * */
        }


        private void button2_Click(object sender, EventArgs e)
        {
            using (var db = new Luokat.PuhelinluetteloContext())
            {
                // Display all Blogs from the database
                var query = from b in db.Henkilot
                            from c in db.Operaattorit
                            where c.Id == b.OperaattoriId
                            select new { b.Nimi, c.OperaattoriNimi };

                Console.WriteLine("All blogs in the database:");
                foreach (var item in query)
                {
                    Console.WriteLine(item.Nimi + " " + item.OperaattoriNimi);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var db = new Luokat.PuhelinluetteloContext())
            {
                string sel = comboBox1.SelectedItem.ToString();
                textBox4.Text = db.Postitoimipaikat.FirstOrDefault(x => x.PostiNumero.Equals(sel)).PostiToimipaikanNimi;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                textBox9.Text = row.Cells[0].Value.ToString();
                textBox8.Text = row.Cells[1].Value.ToString();
                textBox7.Text = row.Cells[2].Value.ToString();
                textBox6.Text = row.Cells[5].Value.ToString();
                comboBox4.SelectedItem = comboBox4.Items[comboBox4.FindStringExact(row.Cells[3].Value.ToString())];
                comboBox3.SelectedItem = comboBox3.Items[comboBox3.FindStringExact(row.Cells[6].Value.ToString())];
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (var db = new Luokat.PuhelinluetteloContext())
            {
                string sel = comboBox4.SelectedItem.ToString();
                textBox5.Text = db.Postitoimipaikat.FirstOrDefault(x => x.PostiNumero.Equals(sel)).PostiToimipaikanNimi;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (var db = new Luokat.PuhelinluetteloContext())
            {
                int id = int.Parse(textBox9.Text);
                var h = db.Henkilot.FirstOrDefault(x => x.HenkiloId == id);

                h.Nimi = textBox8.Text;
                h.LahiOsoite = textBox7.Text;
                string sel = comboBox4.SelectedItem.ToString();
                h.PostiToimipaikkaId = db.Postitoimipaikat.FirstOrDefault(x => x.PostiNumero.Equals(sel)).Id;
                h.PuhNo = textBox6.Text;
                sel = comboBox3.SelectedItem.ToString();
                h.OperaattoriId = db.Operaattorit.FirstOrDefault(x => x.OperaattoriNimi.Equals(sel)).Id;

                db.SaveChanges();

                loadData();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var db = new Luokat.PuhelinluetteloContext())
            {
                int id = int.Parse(textBox9.Text);
                Luokat.Henkilo h = db.Henkilot.FirstOrDefault(x => x.HenkiloId == id);
                db.Henkilot.Remove(h);
                db.SaveChanges();

                loadData();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            using ( var db = new Luokat.PuhelinluetteloContext())
            {
                string temp = comboBox1.SelectedItem.ToString();
                int pId = db.Postitoimipaikat.FirstOrDefault(x=> x.PostiNumero.Equals(temp)).Id;

                temp = comboBox2.SelectedItem.ToString();
                int oId = db.Operaattorit.FirstOrDefault(x => x.OperaattoriNimi.Equals(temp)).Id;

                var h = new Luokat.Henkilo { LahiOsoite = textBox2.Text, Nimi = textBox1.Text, PuhNo = textBox3.Text, PostiToimipaikkaId = pId, OperaattoriId = oId };

                db.Henkilot.Add(h);

                db.SaveChanges();

                loadData();
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            using (var db = new Luokat.PuhelinluetteloContext())
            {
                string sel = comboBox1.SelectedItem.ToString();
                textBox4.Text = db.Postitoimipaikat.FirstOrDefault(x => x.PostiNumero.Equals(sel)).PostiToimipaikanNimi;
            }
        }
    }
}
