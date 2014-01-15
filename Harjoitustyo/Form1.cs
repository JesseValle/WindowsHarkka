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
        private int selected = -1;

        public Form1()
        {
            InitializeComponent();

            comboBox5.SelectedIndex = 0;

            loadData();
        }

        void loadData(bool haku = false)
        {
            comboBox4.Items.Clear();
            comboBox1.Items.Clear();
            comboBox3.Items.Clear();
            comboBox2.Items.Clear();

            using (var db = new Luokat.PuhelinluetteloContext())
            {
                var query = db.Postitoimipaikat.Select(x=>x);

                foreach (var a in query)
                {
                    comboBox4.Items.Add(a.PostiNumero);
                    comboBox1.Items.Add(a.PostiNumero);
                    comboBox1.SelectedIndex = 0;
                    comboBox4.SelectedIndex = 0;
                }

                var query2 = db.Operaattorit.Select(x=>x);

                foreach (var a in query2)
                {
                    comboBox3.Items.Add(a.OperaattoriNimi);
                    comboBox2.Items.Add(a.OperaattoriNimi);
                    comboBox3.SelectedIndex = 0;
                    comboBox2.SelectedIndex = 0;
                }

                if (!haku)
                {
                    var query3 = from a in db.Henkilot
                                 from b in db.Postitoimipaikat
                                 from c in db.Operaattorit
                                 where a.OperaattoriId == c.Id && a.PostiToimipaikkaId == b.Id
                                 select new { a.HenkiloId, a.Nimi, a.LahiOsoite, b.PostiNumero, b.PostiToimipaikanNimi, a.PuhNo, c.OperaattoriNimi };

                    dataGridView1.DataSource = query3.ToList();
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }
                if (!haku && selected >= 0)
                {
                    dataGridView1.Rows[this.selected].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[this.selected].Cells[1];
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

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
            if (!textBox8.Text.Trim().Equals(string.Empty))
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

                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        this.selected = row.Index;
                    }

                    label17.Text = "Yhteystieto päivitetty";

                    loadData();
                }
            }
            else
            {
                label17.Text = "Nimi ei voi olla tyhjä!";
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
            if (!textBox1.Text.Trim().Equals(string.Empty))
            {
                using (var db = new Luokat.PuhelinluetteloContext())
                {
                    string temp = comboBox1.SelectedItem.ToString();
                    int pId = db.Postitoimipaikat.FirstOrDefault(x => x.PostiNumero.Equals(temp)).Id;

                    temp = comboBox2.SelectedItem.ToString();
                    int oId = db.Operaattorit.FirstOrDefault(x => x.OperaattoriNimi.Equals(temp)).Id;

                    var h = new Luokat.Henkilo { LahiOsoite = textBox2.Text, Nimi = textBox1.Text, PuhNo = textBox3.Text, PostiToimipaikkaId = pId, OperaattoriId = oId };

                    db.Henkilot.Add(h);

                    db.SaveChanges();

                    label18.Text = "Yhteystieto lisätty";

                    loadData();
                }
            }
            else
            {
                label18.Text = "Nimi ei voi olla tyhjä!";
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

        private void button5_Click(object sender, EventArgs e)
        {
            loadData(haku: true);
            using (var db = new Luokat.PuhelinluetteloContext())
            {
                if (comboBox5.SelectedIndex == 0)
                {

                    var query = from a in db.Henkilot
                                from b in db.Postitoimipaikat
                                from c in db.Operaattorit
                                where a.OperaattoriId == c.Id && a.PostiToimipaikkaId == b.Id && a.Nimi.Contains(textBox10.Text)
                                select new { a.HenkiloId, a.Nimi, a.LahiOsoite, b.PostiNumero, b.PostiToimipaikanNimi, a.PuhNo, c.OperaattoriNimi };

                    dataGridView1.DataSource = query.ToList();
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                }
                else
                {
                    var query = from a in db.Henkilot
                            from b in db.Postitoimipaikat
                            from c in db.Operaattorit
                            where a.OperaattoriId == c.Id && a.PostiToimipaikkaId == b.Id && a.PuhNo.Contains(textBox10.Text)
                            select new { a.HenkiloId, a.Nimi, a.LahiOsoite, b.PostiNumero, b.PostiToimipaikanNimi, a.PuhNo, c.OperaattoriNimi };

                    dataGridView1.DataSource = query.ToList();
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                }
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!textBox11.Text.Trim().Equals(string.Empty))
            {
                using (var db = new Luokat.PuhelinluetteloContext())
                {
                    if (db.Operaattorit.FirstOrDefault(x => x.OperaattoriNimi.Equals(textBox11.Text)) == null)
                    {
                        var op = new Luokat.Operaattori { OperaattoriNimi = textBox11.Text };
                        db.Operaattorit.Add(op);
                        db.SaveChanges();

                        label20.Text = "Operaattori lisätty";
                    }
                    else
                    {
                        label20.Text = "Tämä operaattori on jo tietokannassa!";
                    }
                }

                loadData();
            }
            else
            {
                label20.Text = "Kenttä ei voi olla tyhjä!";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!textBox12.Text.Trim().Equals(string.Empty) && !textBox13.Text.Trim().Equals(string.Empty))
            {
                using (var db = new Luokat.PuhelinluetteloContext())
                {
                    if (db.Postitoimipaikat.FirstOrDefault(x => x.PostiNumero.Equals(textBox12.Text)) == null)
                    {
                        var pk = new Luokat.PostiToimipaikka { PostiNumero = textBox12.Text, PostiToimipaikanNimi = textBox13.Text };
                        db.Postitoimipaikat.Add(pk);
                        db.SaveChanges();

                        label23.Text = "Postitoimipaikka lisätty";
                    }
                    else
                    {
                        label23.Text = "Tämä postinumero on jo tietokannassa!";
                    }
                }

                loadData();
            }
            else
            {
                label23.Text = "Kumpikaan kenttä ei voi olla tyhjä";
            }
        }
    }
}
