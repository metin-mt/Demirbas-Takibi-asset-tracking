﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace DemirbasTakip
{
    public partial class FormDemirbas : Form
    {
        DataSet ds = new DataSet();
        DataSet dsSehir = new DataSet();
        BindingSource bs = new BindingSource();
        OleDbConnection baglan = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=DemirbasTakip.accdb");
        public FormDemirbas()
        {
            InitializeComponent();
        }
        void verileri_cek()
        {
            string sqlkomutu = "SELECT * FROM demirbas WHERE demirbaskodu like '%" + textBoxFiltreDemirbasKodu.Text + "%' AND demirbasadi like '%" + textBoxFiltreDemirbasAdi.Text + "%' AND demirbasbirimi like '%" + textBoxFiltreDemirbasBirimi.Text + "%'";
            OleDbDataAdapter da = new OleDbDataAdapter(sqlkomutu, baglan);
            ds.Clear();
            da.Fill(ds, "demirbas");

        }

        private void FormDemirbas_Load(object sender, EventArgs e)
        {
            this.demirbasTableAdapter.Fill(this.demirbasTakipDataSet.Demirbas);
            if (baglan.State == ConnectionState.Closed) baglan.Open();

            verileri_cek();
            bs.DataSource = ds.Tables["demirbas"];
            dataGridViewDemirbaslar.DataSource = bs;
            ID_Demirbas.DataBindings.Add("Text", bs, "ID");
            textBoxDemirbasKodu.DataBindings.Add("Text", bs, "DemirbasKodu");
            textBoxDemirbasAdi.DataBindings.Add("Text", bs, "DemirbasAdi");
            textBoxDemirbasBirimi.DataBindings.Add("Text", bs, "DemirbasBirimi");
            textBoxGirenMiktar.DataBindings.Add("Text", bs, "GirenMiktar");
            textBoxCikanMiktar.DataBindings.Add("Text", bs, "CikanMiktar");
            textBoxKalanMiktar.DataBindings.Add("Text", bs, "KalanMiktar");
            buttonKaydet.Visible = false;
        }

        private void FormDemirbas_FormClosed(object sender, FormClosedEventArgs e)
        {
            baglan.Close();
            this.Close();
            FormAnaMenu formAnaMenu = new FormAnaMenu();
            formAnaMenu.Show();
        }

        private void buttonYeniKayit_Click(object sender, EventArgs e)
        {
            buttonKaydet.Visible = true;
            buttonGuncelle.Visible = false;
            buttonSil.Visible = false;
            textBoxDemirbasKodu.Clear();
            textBoxDemirbasAdi.Clear();
            textBoxDemirbasBirimi.Clear();
            textBoxGirenMiktar.Text = "0";
            textBoxCikanMiktar.Text = "0";
            textBoxKalanMiktar.Text = "0";
        }

        private void buttonKaydet_Click(object sender, EventArgs e)
        {
            if (textBoxDemirbasKodu.Text.Trim() != "" && textBoxDemirbasAdi.Text.Trim() != "" && textBoxDemirbasBirimi.Text.Trim() != "")
            {
                OleDbCommand komut = new OleDbCommand();
                komut.Connection = baglan;
                komut.CommandText = "INSERT INTO demirbas (demirbaskodu,demirbasadi,demirbasbirimi,girenmiktar,cikanmiktar,kalanmiktar) Values ('" + textBoxDemirbasKodu.Text + "','" + textBoxDemirbasAdi.Text + "','" + textBoxDemirbasBirimi.Text + "','" + textBoxGirenMiktar.Text + "','" + textBoxCikanMiktar.Text + "','" + textBoxKalanMiktar.Text + "')";
                komut.ExecuteNonQuery();
                MessageBox.Show("Kayıt İşlemi Tamamlandı");
                verileri_cek();
                buttonKaydet.Visible = false;
                buttonGuncelle.Visible = true;
                buttonSil.Visible = true;
                bs.Position = bs.Count;
            }
            else
            {
                MessageBox.Show("İşlem Geçersiz. Kayıt için gerekli alanları boş bırakamazsınız");
            }
        }

        private void buttonGuncelle_Click(object sender, EventArgs e)
        {
            if (textBoxDemirbasKodu.Text.Trim() != "" && textBoxDemirbasAdi.Text.Trim() != "" && textBoxDemirbasBirimi.Text.Trim() != "")
            {
                int pozisyontut = bs.Position;
                OleDbCommand komut = new OleDbCommand();
                komut.Connection = baglan;
                komut.CommandText = "UPDATE demirbas SET demirbaskodu='" + textBoxDemirbasKodu.Text + "', demirbasadi='" + textBoxDemirbasAdi.Text + "', demirbasbirimi='" + textBoxDemirbasBirimi.Text + "', girenmiktar='" + textBoxGirenMiktar.Text + "', cikanmiktar='" + textBoxCikanMiktar.Text + "', kalanmiktar='" + textBoxKalanMiktar.Text + "' WHERE ID=" + ID_Demirbas.Text;
                komut.ExecuteNonQuery();
                MessageBox.Show("Güncelleme İşlemi Tamamlandı");
                verileri_cek();
                bs.Position = pozisyontut;
            }
            else
            {
                MessageBox.Show("İşlem Geçersiz. Güncelleme için gerekli alanları boş bırakamazsınız");
            }
        }

        private void buttonSil_Click(object sender, EventArgs e)
        {
            DialogResult cevap = MessageBox.Show("Silmek İstediğinizden Emin misiniz?", "Uyarı", MessageBoxButtons.YesNo);
            if (cevap == DialogResult.Yes)
            {
                OleDbCommand komut = new OleDbCommand();
                komut.Connection = baglan;
                komut.CommandText = "DELETE FROM demirbas WHERE ID=" + ID_Demirbas.Text;
                komut.ExecuteNonQuery();
                MessageBox.Show("Silme İşlemi Tamamlandı");
                verileri_cek();
            }
        }

        private void buttonIlkKayit_Click(object sender, EventArgs e)
        {
            bs.Position = 0;
        }

        private void buttonGeri_Click(object sender, EventArgs e)
        {
            if (--bs.Position < 0) bs.Position = bs.Count;
        }

        private void buttonIleri_Click(object sender, EventArgs e)
        {
            if (++bs.Position >= bs.Count) bs.Position = 0;
        }

        private void buttonSonKayit_Click(object sender, EventArgs e)
        {
            bs.Position = bs.Count;
        }

        private void textBoxFiltreDemirbasKodu_TextChanged(object sender, EventArgs e)
        {
            verileri_cek();
        }

        private void groupBoxDemirbaslar_Enter(object sender, EventArgs e)
        {

        }
    }
}