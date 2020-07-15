/*
 * Tạ Xuân Quỳnh - 1620325
 * Vũ Đình Đỉnh - 1620040
 * Tên đăng nhập: admin
 * Mật khẩu: admin
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using Program.Resources;

namespace Program
{
    public partial class FormMain : Form
    {
        SQLiteConnection conn = new SQLiteConnection("Data Source = database.db");
        string fileName;
        public FormMain()
        {
            InitializeComponent();
            loadToGrid();
            treeview();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            openFileDialog1.Title = "Chọn hình đại diện";
            openFileDialog1.Filter = "Jpeg file|*.jpg";
        }

        public DataSet load(string sql, SQLiteConnection conn)
        {
            DataSet ds = new DataSet();
            SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);
            da.Fill(ds);
            return ds;
        }
        public void loadToGrid()
        {
            string sql = "select MaNV as [Mã NV],name as [Tên], ngaysinh as [Ngày sinh], gioitinh as [Giới tính], sdt as [SĐT], diachi as [Địa chỉ], Lop as [Lớp] from tbGV";
            DataSet ds = load(sql, conn);
            dataGridView1.DataSource = ds.Tables[0];
            id.DataBindings.Clear();
            ten.DataBindings.Clear();
            dc.DataBindings.Clear();
            sdt.DataBindings.Clear();
            tblop.DataBindings.Clear();
            id.DataBindings.Add("text", ds.Tables[0], "Mã NV");
            id.Enabled = false;
            ten.DataBindings.Add("text", ds.Tables[0], "Tên");
            dc.DataBindings.Add("text", ds.Tables[0], "Địa chỉ");
            sdt.DataBindings.Add("text", ds.Tables[0], "SĐT");
            tblop.DataBindings.Add("text", ds.Tables[0], "Lớp");
        }

        public void loadToLuong(string id)
        {
            string sql = "select NghiCoPhep as [Có phép], NghiKhongPhep as [Không phép], LamThem as [Làm thêm] from tb_Luong where id = '" + id + "'";
            DataSet ds = load(sql, conn);
            tbNghiKoPhep.DataBindings.Clear();
            tbNghiCoPhep.DataBindings.Clear();
            tbLamThem.DataBindings.Clear();
            tbNghiKoPhep.DataBindings.Add("text", ds.Tables[0], "Không phép");
            tbNghiCoPhep.DataBindings.Add("text", ds.Tables[0], "Có phép");
            tbLamThem.DataBindings.Add("text", ds.Tables[0], "Làm thêm");

        }

        public void tinhLuong(string id)
        {
            string sql = "select LuongCoBan as [Lương cơ bản], PhuCap as [Phụ cấp] from tb_Luong where id = '" + id + "'";
            DataSet ds = load(sql, conn);
            tbLuongCB.DataBindings.Clear();
            tbPhuCap.DataBindings.Clear();
            tbLuongCB.DataBindings.Add("text", ds.Tables[0], "Lương cơ bản");
            tbPhuCap.DataBindings.Add("text", ds.Tables[0], "Phụ cấp");
            if (tbLuongCB.Text != "" && tbNghiKoPhep.Text != "" && tbLamThem.Text != "")
            {
                double luongtt = Math.Round(Convert.ToDouble(tbLuongCB.Text) / 26.0 * (26.0 - Convert.ToDouble(tbNghiKoPhep.Text) + Convert.ToDouble(tbLamThem.Text)) + Convert.ToDouble(tbPhuCap.Text));
                tbLuongTT.Text = luongtt.ToString();
            }
        }

        private void SuaGV()
        {
            string ID = id.Text;
            string Name = ten.Text;
            string ngay = datetime.Value.ToString("dd/MM/yyyy");
            string gioitinh = gtcb.Text;
            string diachi = dc.Text;
            string sodt = sdt.Text;
            string lop = tblop.Text;
            string update = string.Format("UPDATE tbGV set name = '{0}', ngaysinh ='{1}',gioitinh='{2}',sdt='{3}', diachi='{4}',Lop ='{5}' where MaNV='{6}'", Name, ngay, gioitinh, sodt, diachi, lop, ID);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(update, conn);
            cmd.ExecuteNonQuery();
            SuaChamCong();
            conn.Close();
            loadToGrid();
            MessageBox.Show("Sửa thành công", "Thông báo");
        }

        private void ThemGV()
        {
            DataTable tb = new DataTable();
            SQLiteDataAdapter da = new SQLiteDataAdapter("select MaNV from tbGV", conn);
            conn.Open();
            da.Fill(tb);
            double[] arrCode = new double[tb.Rows.Count];
            double code = 0;
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                code = int.Parse(tb.Rows[i]["MaNV"].ToString().Remove(0, 2));
                arrCode[i] = code;
            }
            code = arrCode.Max() + 1;
            string ID = id.Text;
            ID = "NV" + code;
            string Name = ten.Text;
            string ngay = datetime.Value.ToString("dd/MM/yyyy");
            string gioitinh = gtcb.Text;
            string diachi = dc.Text;
            string sodt = sdt.Text;
            string lop = tblop.Text;
            string insert = string.Format("INSERT INTO tbGV (MaNV,name,ngaysinh,gioitinh,sdt,diachi,Lop) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", ID, Name, ngay, gioitinh, sodt, diachi, lop);
            SQLiteCommand cmd = new SQLiteCommand(insert, conn);
            cmd.ExecuteNonQuery();
            ThemChamCong(ID);
            conn.Close();
            loadToGrid();
            MessageBox.Show("Thêm thành công", "Thông báo");
        }


        private void btnthem_Click(object sender, EventArgs e)
        {
            checkClick = true;
            btsaveGV.Show();
            bthuyGV.Show();
            btnthem.Enabled = false;
            btnud.Enabled = false;
            btnxoa.Enabled = false;
            dataGridView1.Enabled = false;
            ClearTextBoxes();
        }


        private void btnud_Click(object sender, EventArgs e)
        {
            checkClick = false;
            btsaveGV.Show();
            bthuyGV.Show();
            btnthem.Enabled = false;
            btnud.Enabled = false;
            btnxoa.Enabled = false;
        }

        private void btsaveGV_Click(object sender, EventArgs e)
        {
            if (checkClick == true)
                ThemGV();
            else if (checkClick == false)
                SuaGV();
            btnthem.Enabled = true;
            btnud.Enabled = true;
            btnxoa.Enabled = true;
            btsaveGV.Hide();
            bthuyGV.Hide();
            dataGridView1.Enabled = true;
        }

        private void bthuyGV_Click(object sender, EventArgs e)
        {
            btnthem.Enabled = true;
            btnud.Enabled = true;
            btnxoa.Enabled = true;
            btsaveGV.Hide();
            bthuyGV.Hide();
            dataGridView1.Enabled = true;
            loadToGrid();
        }

        private void ThemChamCong(string idgv)
        {
            string Themluong = string.Format("insert into tb_Luong (id,NghiKhongPhep,NghiCoPhep,LamThem,LuongCoban,PhuCap) values('{0}','{1}','{2}','{3}','{4}','{5}')",idgv, tbNghiKoPhep.Text, tbNghiCoPhep.Text, tbLamThem.Text, tbLuongCB.Text, tbPhuCap.Text);
            SQLiteCommand cmd = new SQLiteCommand(Themluong, conn);
            cmd.ExecuteNonQuery();
        }

        private void SuaChamCong()
        {
            string updateluong = string.Format("update tb_Luong set NghiKhongPhep='{0}', NghiCoPhep='{1}',LamThem='{2}',LuongCoBan='{3}',PhuCap='{4}' where id ='{5}'", tbNghiKoPhep.Text, tbNghiCoPhep.Text, tbLamThem.Text, tbLuongCB.Text, tbPhuCap.Text, id.Text);
            SQLiteCommand cmd = new SQLiteCommand(updateluong, conn);
            cmd.ExecuteNonQuery();
        }

        private void XoaChamCong()
        {
            string xoaluong = string.Format("delete from tb_Luong where id ='{5}'", tbNghiKoPhep.Text, tbNghiCoPhep.Text, tbLamThem.Text, tbLuongCB.Text, tbPhuCap.Text, id.Text);
            SQLiteCommand cmd = new SQLiteCommand(xoaluong, conn);
            cmd.ExecuteNonQuery();
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận xóa?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string ID = id.Text;
                string delete = string.Format("DELETE FROM tbGV where MaNV = '{0}'", ID);
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(delete, conn);
                cmd.ExecuteNonQuery();
                XoaChamCong();
                conn.Close();
                loadToGrid();
                MessageBox.Show("Xóa thành công", "Thông báo");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int numrow;
            numrow = e.RowIndex;
            if (dataGridView1.Rows.Count > 1 && numrow >= 0 && id.Text != "")
            {
                datetime.Value = DateTime.ParseExact(dataGridView1.Rows[numrow].Cells[2].Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                gtcb.Text = dataGridView1.Rows[numrow].Cells[3].Value.ToString();
                loadToLuong(id.Text);
                tinhLuong(id.Text);
            }
        }



        public void Loaddata(SQLiteDataAdapter da)
        {
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            id.DataBindings.Clear();
            ten.DataBindings.Clear();
            dc.DataBindings.Clear();
            sdt.DataBindings.Clear();
            id.DataBindings.Add("text", ds.Tables[0], "Mã NV");
            id.Enabled = false;
            ten.DataBindings.Add("text", ds.Tables[0], "Tên");
            dc.DataBindings.Add("text", ds.Tables[0], "Địa chỉ");
            sdt.DataBindings.Add("text", ds.Tables[0], "SĐT");
        }



        private DataTable selectData(string sql, SQLiteConnection conn)
        {
            DataTable dt = new DataTable();
            SQLiteDataAdapter da = new SQLiteDataAdapter(sql, conn);
            da.Fill(dt);
            return dt;
        }

        void treeview()
        {

            DataTable dt = selectData("select * from tbHS", conn);
            List<HocSinh> lHS = new List<HocSinh>();

            foreach (DataRow dataRow in dt.Rows)
            {
                HocSinh hs = new HocSinh(dataRow);
                lHS.Add(hs);
            }

            foreach (HocSinh hs in lHS)
            {
                TreeNode node = new TreeNode();
                node.Tag = hs;
                node.Text = hs.name;
                treeView1.Nodes.Add(node);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode nodeSelected = this.treeView1.SelectedNode;
            HocSinh nvSelected = (HocSinh)nodeSelected.Tag;
            tbmahs.Text = nvSelected.hsID;
            tbhoten.Text = nvSelected.name;
            txtlop.Text = nvSelected.classroom;
            tbnamhoc.Text = nvSelected.year.ToString();
            dtnamsinh.Value = DateTime.ParseExact(nvSelected.dOB.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            tbcannang.Text = nvSelected.weight.ToString();
            tbchieucao.Text = nvSelected.height.ToString();
            double bmidata = Math.Round(Convert.ToDouble(tbcannang.Text) / (Convert.ToDouble(tbchieucao.Text) * 0.01 * Convert.ToInt32(tbchieucao.Text) * 0.01), 1);
            tbbmi.Text = Convert.ToString(bmidata);
            cbgioitinh.Text = nvSelected.gender.ToString();
            tbdiachi.Text = nvSelected.address.ToString();
            tbcha.Text = nvSelected.dName.ToString();
            tbme.Text = nvSelected.mName.ToString();
            tbnghecha.Text = nvSelected.dJob.ToString();
            tbngheme.Text = nvSelected.mJob.ToString();
            tbnscha.Text = nvSelected.dYOB.ToString();
            tbnsme.Text = nvSelected.mYOB.ToString();
            tbsdt.Text = nvSelected.phone.ToString();
            dtvaohoc.Value = DateTime.ParseExact(nvSelected.yearj.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (bmidata < 18.5)
            {
                tbdanhgia.Text = "Gầy";
            }
            else if (bmidata >= 18.5 && bmidata < 22.9)
                tbdanhgia.Text = "Bình thường";
            else if (bmidata >= 23 && bmidata < 24.9)
                tbdanhgia.Text = "Thừa cân";
            else if (bmidata >= 25 && bmidata < 29.9)
                tbdanhgia.Text = "Béo phì độ I";
            else if (bmidata >= 30 && bmidata < 34.9)
                tbdanhgia.Text = "Béo phì độ II";
            else if (bmidata >= 35)
                tbdanhgia.Text = "Béo phì độ III";
            loadIMG();
        }

        private void btsuahs_Click(object sender, EventArgs e)
        {
            checkClick = false;
            btSave.Show();
            btthemhs.Enabled = false;
            btxoahs.Enabled = false;
            btsuahs.Enabled = false;
            btcancel.Show();
            btselectimg.Show();
        }

        private void btthem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            checkClick = true;
            btthemhs.Enabled = false;
            btxoahs.Enabled = false;
            btsuahs.Enabled = false;
            btSave.Show();
            ClearTextBoxes();
            treeView1.Enabled = false;
            btcancel.Show();
            btselectimg.Show();
        }

        private void btcancel_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            treeView1.Enabled = true;
            btcancel.Hide();
            btSave.Hide();
            btthemhs.Enabled = true;
            btsuahs.Enabled = true;
            btxoahs.Enabled = true;
            treeview();
        }

        private void btxoahs_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xác nhận xóa?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string mahs = tbmahs.Text;
                string deletehs = string.Format("DELETE FROM tbHS where id = '{0}'", mahs);
                conn.Open();
                SQLiteCommand cmd = new SQLiteCommand(deletehs, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                treeView1.Nodes.Clear();
                treeview();
                MessageBox.Show("Xóa thành công", "Thông báo");
            }
        }
        private bool checkClick = false;
        private void btSave_Click(object sender, EventArgs e)
        {
            if (checkClick == true)
            {
                ThemHS();
            }
            else if (checkClick == false)
            {
                SuaHS();
            }
            btthemhs.Enabled = true;
            btxoahs.Enabled = true;
            btsuahs.Enabled = true;
            btSave.Hide();
            btcancel.Hide();
            btselectimg.Hide();
            treeView1.Enabled = true;
        }

        private void ClearTextBoxes()
        {
            Action<Control.ControlCollection> func = null;

            func = (controls) =>
            {
                foreach (Control control in controls)
                    if (control is TextBox)
                        (control as TextBox).Clear();
                    else
                        func(control.Controls);
            };

            func(Controls);
        }

        private void ThemHS()
        {
            byte[] arr;
            conn.Open();
            arr = covertToBinary(pictureBox1.Image);         
            DataTable tb = new DataTable();
            SQLiteDataAdapter da = new SQLiteDataAdapter("select id from tbHS", conn);
            da.Fill(tb);
            double[] arrCode = new double[tb.Rows.Count];
            double code = 0;
            for (int i = 0; i < tb.Rows.Count; i++)
            {
                code = int.Parse(tb.Rows[i]["id"].ToString().Remove(0, 2));
                arrCode[i] = code;
            }
            code = arrCode.Max() + 1;
            string mahs = tbmahs.Text;
            mahs = "HS" + code;
            string tenhs = tbhoten.Text;
            string lophs = txtlop.Text;
            string namhoc = tbnamhoc.Text;
            string ngaysinhhs = dtnamsinh.Value.ToString("dd/MM/yyyy");
            string cannang = tbcannang.Text;
            string chieucao = tbchieucao.Text;
            string gths = cbgioitinh.Text;
            string diachihs = tbdiachi.Text;
            string tencha = tbcha.Text;
            string tenme = tbme.Text;
            string nghecha = tbnghecha.Text;
            string ngheme = tbngheme.Text;
            string nscha = tbnscha.Text;
            string nsme = tbnsme.Text;
            string sdths = tbsdt.Text;
            string ngayvaohoc = dtvaohoc.Value.ToString("dd/MM/yyyy");
            string inserths = string.Format("INSERT INTO tbHS (id,name,lop,namhoc,ngaysinh,cannang,chieucao,gioitinh,diachi,tencha,tenme,nghecha,ngheme,nscha,nsme,sdt,ngayvaohoc,img) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}',(@arr))", mahs, tenhs, lophs, namhoc, ngaysinhhs, cannang, chieucao, gths, diachihs, tencha, tenme, nghecha, ngheme, nscha, nsme, sdths, ngayvaohoc);
            SQLiteCommand cmd = new SQLiteCommand(inserths, conn);
            cmd.Parameters.Add("@arr", DbType.Binary).Value = arr;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Thêm thành công", "Thông báo");
            conn.Close();          
            treeView1.Nodes.Clear();
            treeview();
           
        }

        private void SuaHS()
        {
            string mahs = tbmahs.Text;
            string tenhs = tbhoten.Text;
            string lophs = txtlop.Text;
            string namhoc = tbnamhoc.Text;
            string ngaysinhhs = dtnamsinh.Value.ToString("dd/MM/yyyy");
            string cannang = tbcannang.Text;
            string chieucao = tbchieucao.Text;
            double bmidata = Convert.ToDouble(cannang) / (Convert.ToDouble(chieucao) * 0.01 * Convert.ToInt32(chieucao) * 0.01);
            string bmi = Convert.ToString(bmidata);
            string gths = cbgioitinh.Text;
            string diachihs = tbdiachi.Text;
            string tencha = tbcha.Text;
            string tenme = tbme.Text;
            string nghecha = tbnghecha.Text;
            string ngheme = tbngheme.Text;
            string nscha = tbnscha.Text;
            string nsme = tbnsme.Text;
            string sdths = tbsdt.Text;
            string ngayvaohoc = dtvaohoc.Value.ToString("dd/MM/yyyy");
            string updatehs = string.Format("UPDATE tbHS set name ='{0}',lop='{1}',namhoc='{2}',ngaysinh='{3}',cannang='{4}',chieucao='{5}',gioitinh='{6}',diachi='{7}',tencha='{8}',tenme='{9}',nghecha='{10}',ngheme='{11}',nscha='{12}',nsme='{13}',sdt='{14}',ngayvaohoc='{15}' where id='{16}'", tenhs, lophs, namhoc, ngaysinhhs, cannang, chieucao, gths, diachihs, tencha, tenme, nghecha, ngheme, nscha, nsme, sdths, ngayvaohoc, mahs);
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(updatehs, conn);
            cmd.ExecuteNonQuery();
            treeView1.Nodes.Clear();
            treeview();
            conn.Close();
            SaveIMG();
            MessageBox.Show("Sửa thành công", "Thông báo");
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void tbsearchhs_TextChanged(object sender, EventArgs e)
        {
            if (cbsearchhs.Text == "Mã học sinh")
            {
                string sql = "select*from tbHS where id like '%" + tbsearchhs.Text + "%'";
                selectSearch(sql, conn);
            }
            else if (cbsearchhs.Text == "Tên")
            {
                string sql = "select*from tbHS where name like '%" + tbsearchhs.Text + "%'";
                selectSearch(sql, conn);
            }
            else if (cbsearchhs.Text == "Lớp")
            {
                string sql = "select*from tbHS where lop like '%" + tbsearchhs.Text + "%'";
                selectSearch(sql, conn);
            }
            else if (cbsearchhs.Text == "Giới tính")
            {
                string sql = "select*from tbHS where gioitinh like '%" + cbgenersr.Text + "%'";
                selectSearch(sql, conn);
            }
        }

        public void selectSearch(string sql, SQLiteConnection conn)
        {
            treeView1.Nodes.Clear();
            DataTable dt = selectData(sql, conn);
            List<HocSinh> lHS = new List<HocSinh>();

            foreach (DataRow dataRow in dt.Rows)
            {
                HocSinh hs = new HocSinh(dataRow);
                lHS.Add(hs);
            }

            foreach (HocSinh hs in lHS)
            {
                TreeNode node = new TreeNode();
                node.Tag = hs;
                node.Text = hs.name;
                treeView1.Nodes.Add(node);
            }
        }

        private void cbsearchhs_TextChanged(object sender, EventArgs e)
        {
            tbsearchhs.Enabled = true;
            if (cbsearchhs.Text == "Giới tính")
            {
                cbgenersr.Show();
                tbsearchhs.Hide();
            }
            else
            {
                cbgenersr.Hide();
                tbsearchhs.Show();
            }

        }
        private void cbgenersr_TextChanged(object sender, EventArgs e)
        {
            string sql = "select*from tbHS where gioitinh like '%" + cbgenersr.Text + "%'";
            selectSearch(sql, conn);
        }

        private void tbsearchnv_TextChanged(object sender, EventArgs e)
        {
            if (cbsearchnv.Text == "Mã nhân viên")
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter("select MaNV as [Mã NV],name as [Tên], ngaysinh as [Ngày sinh] , gioitinh as [Giới tính], sdt as [SĐT], diachi as [Địa chỉ] from tbGV where MaNV like '%" + tbsearchnv.Text + "%'", conn);
                Loaddata(da);
            }
            else if (cbsearchnv.Text == "Tên")
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter("select MaNV as [Mã NV],name as [Tên], ngaysinh as [Ngày sinh] , gioitinh as [Giới tính], sdt as [SĐT], diachi as [Địa chỉ] from tbGV where name like '%" + tbsearchnv.Text + "%'", conn);
                Loaddata(da);
            }
            else if (cbsearchnv.Text == "Lớp")
            {
                SQLiteDataAdapter da = new SQLiteDataAdapter("select MaNV as [Mã NV],name as [Tên], ngaysinh as [Ngày sinh] , gioitinh as [Giới tính], sdt as [SĐT], diachi as [Địa chỉ] from tbGV where Lop like '%" + tbsearchnv.Text + "%'", conn);
                Loaddata(da);
            }
        }

        private void cbgtnv_TextChanged(object sender, EventArgs e)
        {
            SQLiteDataAdapter da = new SQLiteDataAdapter("select MaNV as [Mã NV],name as [Tên], ngaysinh as [Ngày sinh] , gioitinh as [Giới tính], sdt as [SĐT], diachi as [Địa chỉ] from tbGV where gioitinh like '%" + cbgtnv.Text + "%'", conn);
            Loaddata(da);

        }

        private void cbsearchnv_TextChanged(object sender, EventArgs e)
        {
            tbsearchnv.Enabled = true;
            cbgtnv.Enabled = true;
            if (cbsearchnv.Text == "Giới tính")
            {
                cbgtnv.Show();
                tbsearchnv.Hide();
            }
            else
            {
                cbgtnv.Hide();
                tbsearchnv.Show();
            }
        }

        private void btselectimg_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(fileName);

            }

        }

        private void SaveIMG()
        {
            byte[] arr;
            conn.Open();
            arr = covertToBinary(pictureBox1.Image);
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = string.Format("update tbHS set img = (@arr) where id='" + tbmahs.Text + "'");
            cmd.Parameters.Add("@arr", DbType.Binary).Value = arr;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception exc1)
            {
                MessageBox.Show(exc1.Message);
            }
            conn.Close();
        }

        byte[] covertToBinary(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {   if (pictureBox1.Image != null)
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                return ms.ToArray();
            }
        }

        Image coverToImg(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream(data))
            {
                return Image.FromStream(ms);
            }
        }


        private void loadIMG()
        {
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand("select img from tbHS where id='" + tbmahs.Text + "'", conn);
            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            if (ds.Tables[0].Rows.Count >0)
            {
                MemoryStream ms = new MemoryStream((byte[])ds.Tables[0].Rows[0]["img"]);
                if (ms.Length > 0)
                {
                    pictureBox1.Image = new Bitmap(ms);
                }
                else
                    pictureBox1.Image = null;
            }
            conn.Close();
        }
        private void Form2_Shown(object sender, EventArgs e)
        {
            datetime.Value = DateTime.ParseExact(dataGridView1.Rows[0].Cells[2].Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            gtcb.Text = dataGridView1.Rows[0].Cells[3].Value.ToString();
            if(treeView1.Nodes.Count != 0)
            {
                treeView1.SelectedNode = treeView1.Nodes[0];
            }
            tbmahs.Enabled = false;
            tbbmi.Enabled = false;
            tbdanhgia.Enabled = false;
            cbgenersr.Hide();
            tbsearchhs.Enabled = false;
            btSave.Hide();
            btsaveGV.Hide();
            btcancel.Hide();
            btselectimg.Hide();
            bthuyGV.Hide();
        }
    }
}