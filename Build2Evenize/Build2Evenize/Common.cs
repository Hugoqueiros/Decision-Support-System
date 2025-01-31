﻿using System;
using System.Collections;
using System.Data.SqlClient;
using System.Windows.Forms;

public class Common
{
    public SqlConnection con;
    public string server = @"build2evenize.database.windows.net";
    public string database = "build2evenize";
    public string user = @"ispg4259";
    public string pass = "BUILD2evenize";
    public SqlCommand cmd;
    public SqlDataReader dr;
    public void Connection()
    {
        String str = $"server={server};database={database};UID={user};password={pass};Trusted_Connection=False;Encrypt=True;MultipleActiveResultSets=true;";
        con = new SqlConnection(str);
        con.Open();
    }
	public void Filters(string table, string columnName, ComboBox comboBox)
    {
        // query to get columnname from table and fill in the combobox items
        string query = "select distinct " + columnName + " from " + table;
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            //add every row found on database to combobox
            string name = (string)dr[columnName];
            comboBox.Items.Add(name);
        }
        dr.Close();
        dr.Dispose();
    }
    public void InstitutionCountry(string institution, Label label)
    {
        string query = "select country from Institution where name LIKE '" + institution + "'";
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            label.Text = dr.GetString(0);
        }
        dr.Close();
        dr.Dispose();
    }
    public void Fill(string query, ComboBox cb1, ComboBox cb2, ComboBox cb3)
    {
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            //add every row found on database to combobox
            string name = (string)dr[0];
            cb1.Items.Add(name);
            cb2.Items.Add(name);
            cb3.Items.Add(name);
        }
        dr.Close();
        dr.Dispose();
    }
    public void Switcher(int check, string query, ComboBox cb1, ComboBox cb2, ComboBox cb3, Button Delete1, Button Delete2, Button Delete3, Button Add1, Button Add2)
    {
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        int i = 0;
        ArrayList name_list = new ArrayList();
        ArrayList id_list = new ArrayList();

        while (dr.Read())
        {
            int id = (int)dr[0];
            string name = (string)dr[1];
            name_list.Add(name);
            id_list.Add(id);
            i++;
        }
        dr.Close();
        dr.Dispose();
        if (id_list.Count != 0)
        {
            //check - 0-institution data 1-Hard Skills data 2-Soft Skills data
            if (check == 0)
            {
                Index("select name from Institution where institution_id = " + (int)id_list[0], cb1);
            }
            else if (check == 1)
            {
                Index("select name from Tech where tech_id = " + (int)id_list[0], cb1);
            }
            else if (check == 2)
            {
                Index("select name from Social_Skill where social_skill_id = " + (int)id_list[0], cb1);
            }
            //i - check if there is 0,1,2 or 3 chosen options
            switch (i)
            {
                case 0:
                    break;
                case 1:
                    cb2.Visible = false;
                    cb3.Visible = false;
                    Add1.Visible = true;
                    Add2.Visible = false;
                    Delete1.Visible = true;
                    break;
                case 2:
                    if (check == 0)
                    {
                        
                        Index("select name from Institution where institution_id = " + (int)id_list[1], cb2);
                    }
                    else if (check == 1)
                    {
                        Index("select name from Tech where tech_id = " + (int)id_list[1], cb2);
                    }
                    else if (check == 2)
                    {
                        Index("select name from Social_Skill where social_skill_id = " + (int)id_list[1], cb2);
                    }
                    cb1.Visible = true;
                    cb2.Visible = true;
                    cb3.Visible = false;
                    Delete1.Visible = false;
                    Delete2.Visible = true;
                    Add1.Visible = false;
                    Add2.Visible = true;
                    break;
                case 3:
                    if (check == 0)
                    {
                        Index("select name from Institution where institution_id = " + (int)id_list[1], cb2);
                        Index("select name from Institution where institution_id = " + (int)id_list[2], cb3);
                    }
                    else if (check == 1)
                    {
                        Index("select name from Tech where tech_id = " + (int)id_list[1], cb2);
                        Index("select name from Tech where tech_id = " + (int)id_list[2], cb3);
                    }
                    else if (check == 2)
                    {
                        Index("select name from Social_Skill where social_skill_id = " + (int)id_list[1], cb2);
                        Index("select name from Social_Skill where social_skill_id = " + (int)id_list[2], cb3);
                    }

                    cb1.Visible = true;
                    cb2.Visible = true;
                    cb3.Visible = true;
                    Add1.Visible = false;
                    Add2.Visible= false;
                    Delete1.Visible = false;
                    Delete2.Visible= false;
                    Delete3.Visible = true;
                    break;
            }

        }
    }
    public void Index(string query, ComboBox cb)
    {
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            cb.SelectedItem = dr.GetString(0);
        }
        dr.Close();
        dr.Dispose();
    }
    public void UpdateProject(int id, string name, string desc, int nr, DateTime start, DateTime end, string institution, string area, string partner1, string partner2, string partner3, string tech1, string tech2, string tech3, string sk1, string sk2, string sk3)
    {
        int institution_id=0, area_id=0, db_id1=0, db_id2 = 0, db_id3 = 0;
        string db_name1 = null, db_name2 = null, db_name3 = null, query;

        
        query = "SELECT institution_id from Institution where name LIKE '" + institution + "'";
        SqlCommand cmd = new SqlCommand(query, con);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            institution_id = (dr.GetInt32(0));
        }
        dr.Close();
        dr.Dispose();

        query = "SELECT area_id from Area where name like '" + area + "'";
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            area_id = (dr.GetInt32(0));
        }
        dr.Close();
        dr.Dispose();
        if (id == 0)
        {
            query = "INSERT INTO Project (name,[desc],nr_students,date_start,date_end,institution_id) VALUES ('" + name + "' , '" + desc + "' , " + nr + " ,'" + start.ToString("yyyy-MM-dd") + "' ,'" + end.ToString("yyyy-MM-dd") + "' ," + Get_Id("select institution_id from Institution where name LIKE '" + institution + "'") + ");";
            SqlCommand cmd1 = new SqlCommand(query, con);
            cmd1.ExecuteNonQuery();


            query = "SELECT project_id FROM Project WHERE name='"+name+"' AND desc='"+desc+"' AND nr_students="+nr+" AND date_start = "+ start.ToString("yyyy-MM-dd") + " AND date_end = "+ end.ToString("yyyy-MM-dd") + " AND institution_id "+ Get_Id("select institution_id from Institution where name LIKE '" + institution + "'") + "";
            cmd1 = new SqlCommand(query, con);
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                id = (dr.GetInt32(0));
            }

            query = "INSERT INTO Project_Area (project_id,area_id) VALUES (" + Get_Id("select max(project_id) from Project") + "," + area_id + ")";
            cmd1 = new SqlCommand(query, con);
            cmd1.ExecuteNonQuery();

        }
        //____________________________________________________________________________________________________________________________________________________________________
        // PARTNER UPDATES AND ADDITIONS
        int count =0;
        query = "select PP.institution_id, I.name, PP.partner_id from Project_Partner PP JOIN Institution I on PP.institution_id = I.institution_id where project_id = '" + id + "'";
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            count++;
            switch (count)
            {
                case 0:
                    break;
                case 1:
                    db_id1 = dr.GetInt32(2);
                    db_name1 = dr.GetString(1);
                    break;
                case 2:
                    db_id2 = dr.GetInt32(2);
                    db_name2 = dr.GetString(1);
                    break;
                case 3:
                    db_id3 = dr.GetInt32(2);
                    db_name3 = dr.GetString(1);
                    break;
            }
        }
        dr.Close();
        dr.Dispose();
       

        if (count == 0 && partner1 != "")
        {
            query = "INSERT INTO Project_Partner (project_id, institution_id) VALUES (" + id + "," + Get_Id("select institution_id from Institution where name LIKE '" + partner1 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }else if (count == 1 && partner2 != "")
        {
            query = "INSERT INTO Project_Partner (project_id, institution_id) VALUES (" + id + "," + Get_Id("select institution_id from Institution where name LIKE '" + partner2 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }else if (count == 2 && partner3 != "")
        {
            query = "INSERT INTO Project_Partner (project_id, institution_id) VALUES (" + id + "," + Get_Id("select institution_id from Institution where name LIKE '" + partner3 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        if (partner1 != db_name1)
            if (partner1 != "") {
                query = "UPDATE Project_Partner SET institution_id = " + Get_Id("select institution_id from Institution where name LIKE '" + partner1 + "'") +
                            " WHERE partner_id = " + db_id1 + ";";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            else {
                query = "DELETE FROM Project_Partner WHERE partner_id = " + db_id1 + "";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
        if (partner2 != db_name2)
            if (partner2 != "")
            {
                query = "UPDATE Project_Partner SET institution_id = " + Get_Id("select institution_id from Institution where name LIKE '" + partner2 + "'") +
                            " WHERE partner_id = " + db_id2 + ";";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            else
            {
                query = "DELETE FROM Project_Partner WHERE partner_id = '" + db_id2 + "'";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
        if (partner3 != db_name3)
            if (partner3 != "")
            {
                query = "UPDATE Project_Partner SET institution_id = " + Get_Id("select institution_id from Institution where name LIKE '" + partner3 + "'") +
                            " WHERE partner_id = " + db_id3 + ";";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            else
            {
                query = "DELETE FROM Project_Partner WHERE partner_id = '" + db_id3 + "'";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }



        //____________________________________________________________________________________________________________________________________________________________________
        // HARD SKILLS

        count = 0;
        query = "select PT.tech_id, T.name, PT.project_tech_id from Project_Tech PT JOIN Tech T on PT.tech_id = T.tech_id where PT.project_id = '" + id + "'";
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            count++;
            switch (count)
            {
                case 0:
                    break;
                case 1:
                    db_id1 = dr.GetInt32(2);
                    db_name1 = dr.GetString(1);
                    break;
                case 2:
                    db_id2 = dr.GetInt32(2);
                    db_name2 = dr.GetString(1);
                    break;
                case 3:
                    db_id3 = dr.GetInt32(2);
                    db_name3 = dr.GetString(1);
                    break;
            }
        }
        dr.Close();
        dr.Dispose();


        if (count == 0 && tech1 != "")
        {
            query = "INSERT INTO Project_Tech (project_id, tech_id) VALUES (" + id + "," + Get_Id("select tech_id from Tech where name LIKE '" + tech1 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        else if (count == 1 && tech2 != "")
        {
            query = "INSERT INTO Project_Tech (project_id, tech_id) VALUES (" + id + "," + Get_Id("select tech_id from Tech where name LIKE '" + tech2 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        else if (count == 2 && tech3 != "")
        {
            query = "INSERT INTO Project_Tech (project_id, tech_id) VALUES (" + id + "," + Get_Id("select tech_id from Tech where name LIKE '" + tech3 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        if (tech1 != db_name1)
            if (tech1 != "")
            {
                query = "UPDATE Project_Tech SET tech_id = " + Get_Id("select tech_id from Tech where name LIKE '" + tech1 + "'") +
                            " WHERE project_tech_id = " + db_id1 + ";";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            else
            {
                query = "DELETE FROM Project_Tech WHERE project_tech_id = " + db_id1 + "";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
        if (tech2 != db_name2)
            if (tech2 != "")
            {
                query = "UPDATE Project_Tech SET tech_id = " + Get_Id("select tech_id from Tech where name LIKE '" + tech2 + "'") +
                            " WHERE project_tech_id = " + db_id2 + ";";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            else
            {
                query = "DELETE FROM Project_Tech WHERE project_tech_id = '" + db_id2 + "'";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
        if (tech3 != db_name3)
            if (tech3 != "")
            {
                query = "UPDATE Project_Tech SET tech_id = " + Get_Id("select tech_id from Tech where name LIKE '" + tech3 + "'") +
                            " WHERE project_tech_id = " + db_id3 + ";";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            else
            {
                query = "DELETE FROM Project_Tech WHERE project_tech_id = '" + db_id3 + "'";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }

        //____________________________________________________________________________________________________________________________________________________________________
        // SOFT SKILLS

        count = 0;
        query = "select PS.social_skill_id, SK.name, PS.project_sk_id from Project_SK PS JOIN Social_Skill SK on PS.social_skill_id = SK.social_skill_id where PS.project_id = '" + id + "'";
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            count++;
            switch (count)
            {
                case 0:
                    break;
                case 1:
                    db_id1 = dr.GetInt32(2);
                    db_name1 = dr.GetString(1);
                    break;
                case 2:
                    db_id2 = dr.GetInt32(2);
                    db_name2 = dr.GetString(1);
                    break;
                case 3:
                    db_id3 = dr.GetInt32(2);
                    db_name3 = dr.GetString(1);
                    break;
            }
        }
        dr.Close();
        dr.Dispose();


        if (count == 0 && sk1 != "")
        {
            query = "INSERT INTO Project_SK (project_id, social_skill_id) VALUES (" + id + "," + Get_Id("select social_skill_id from Social_Skill where name LIKE '" + sk1 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        else if (count == 1 && sk2 != "")
        {
            query = "INSERT INTO Project_SK (project_id, social_skill_id) VALUES (" + id + "," + Get_Id("select social_skill_id from Social_Skill where name LIKE '" + sk2 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        else if (count == 2 && sk3 != "")
        {
            query = "INSERT INTO Project_SK (project_id, social_skill_id) VALUES (" + id + "," + Get_Id("select social_skill_id from Social_Skill where name LIKE '" + sk3 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        if (sk1 != db_name1)
            if (sk1 != "")
            {
                query = "UPDATE Project_SK SET social_skill_id = " + Get_Id("select social_skill_id from Social_Skill where name LIKE '" + sk1 + "'") +
                            " WHERE project_sk_id = " + db_id1 + ";";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            else
            {
                query = "DELETE FROM Project_SK WHERE project_sk_id = " + db_id1 + "";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
        if (sk2 != db_name2)
            if (sk2 != "")
            {
                query = "UPDATE Project_SK SET social_skill_id = " + Get_Id("select social_skill_id from Social_Skill where name LIKE '" + sk2 + "'") +
                            " WHERE project_sk_id = " + db_id2 + ";";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            else
            {
                query = "DELETE FROM Project_SK WHERE project_sk_id = '" + db_id2 + "'";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
        if (sk3 != db_name3)
            if (sk3 != "")
            {
                query = "UPDATE Project_SK SET social_skill_id = " + Get_Id("select social_skill_id from Social_Skill where name LIKE '" + sk3 + "'") +
                            " WHERE project_sk_id = " + db_id3 + ";";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }
            else
            {
                query = "DELETE FROM Project_SK WHERE project_sk_id = '" + db_id3 + "'";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }


        //UPDATE OF SIMPLE PROJECT DATA - NAME, DESCRIPTION, ETC.
        query = "UPDATE Project SET name = '" + name + "', [desc] = '" + desc + "', nr_students = " + nr + ", " +
                        "date_start =  '" + start.ToString("yyyy-MM-dd") + "', date_end = '" + end.ToString("yyyy-MM-dd") + "', institution_id=" + institution_id +
                        "WHERE project_id = " + id + ";" +
                "UPDATE Project_Area SET area_id = " + area_id +
                        "WHERE project_id = " + id + ";";

        cmd = new SqlCommand(query, con);
        cmd.ExecuteNonQuery();
    }

    public void InsertProject(int id, string name, string desc, int nr, DateTime start, DateTime end, string institution, string area, string partner1, string partner2, string partner3, string tech1, string tech2, string tech3, string sk1, string sk2, string sk3)
    {
        int institution_id = 0, area_id = 0, db_id1 = 0, db_id2 = 0, db_id3 = 0;
        string db_name1 = null, db_name2 = null, db_name3 = null, query;


        query = "SELECT institution_id from Institution where name LIKE '" + institution + "'";
        SqlCommand cmd = new SqlCommand(query, con);
        SqlDataReader dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            institution_id = (dr.GetInt32(0));
        }
        dr.Close();
        dr.Dispose();

        query = "SELECT area_id from Area where name like '" + area + "'";
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            area_id = (dr.GetInt32(0));
        }
        dr.Close();
        dr.Dispose();
        
        query = "INSERT INTO Project (name,[desc],nr_students,date_start,date_end,institution_id) VALUES ('" + name + "' , '" + desc + "' , " + nr + " ,'" + start.ToString("yyyy-MM-dd") + "' ,'" + end.ToString("yyyy-MM-dd") + "' ," + Get_Id("select institution_id from Institution where name LIKE '" + institution + "'") + ");";
        SqlCommand cmd1 = new SqlCommand(query, con);
        cmd1.ExecuteNonQuery();

        dr.Close();
        dr.Dispose();

        int newid = 0;
        query = "SELECT project_id FROM Project WHERE name='" + name + "' AND nr_students=" + nr + " AND date_start = '" + start.ToString("yyyy-MM-dd") + "' AND date_end = '" + end.ToString("yyyy-MM-dd") + "' AND institution_id = " + Get_Id("select institution_id from Institution where name LIKE '" + institution + "'") + "";
        cmd1 = new SqlCommand(query, con);
        dr = cmd1.ExecuteReader();

        if (dr.Read())
        {
            id = (dr.GetInt32(0));
        }

        query = "INSERT INTO Project_Area (project_id,area_id) VALUES (" + Get_Id("select max(project_id) from Project") + "," + area_id + ")";
        cmd1 = new SqlCommand(query, con);
        cmd1.ExecuteNonQuery();

        
        //____________________________________________________________________________________________________________________________________________________________________
        // PARTNER UPDATES AND ADDITIONS


        if (partner1 != "")
        {          
            query = "INSERT INTO Project_Partner (project_id, institution_id) VALUES (" + id + "," + Get_Id("select institution_id from Institution where name LIKE '" + partner1 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        if (partner2 != "")
        {
            query = "INSERT INTO Project_Partner (project_id, institution_id) VALUES (" + id + "," + Get_Id("select institution_id from Institution where name LIKE '" + partner2 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        if (partner3 != "")
        {
            query = "INSERT INTO Project_Partner (project_id, institution_id) VALUES (" + id + "," + Get_Id("select institution_id from Institution where name LIKE '" + partner3 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }



        //____________________________________________________________________________________________________________________________________________________________________
        // HARD SKILLS

      
        if (tech1 != "")
        {
            query = "INSERT INTO Project_Tech (project_id, tech_id) VALUES (" + id + "," + Get_Id("select tech_id from Tech where name LIKE '" + tech1 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        if (tech2 != "")
        {
            query = "INSERT INTO Project_Tech (project_id, tech_id) VALUES (" + id + "," + Get_Id("select tech_id from Tech where name LIKE '" + tech2 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        if (tech3 != "")
        {
            query = "INSERT INTO Project_Tech (project_id, tech_id) VALUES (" + id + "," + Get_Id("select tech_id from Tech where name LIKE '" + tech3 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }


        //____________________________________________________________________________________________________________________________________________________________________
        // SOFT SKILLS

       
        if (sk1 != "")
        {
            query = "INSERT INTO Project_SK (project_id, social_skill_id) VALUES (" + id + "," + Get_Id("select social_skill_id from Social_Skill where name LIKE '" + sk1 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        if (sk2 != "")
        {
            query = "INSERT INTO Project_SK (project_id, social_skill_id) VALUES (" + id + "," + Get_Id("select social_skill_id from Social_Skill where name LIKE '" + sk2 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }
        if (sk3 != "")
        {
            query = "INSERT INTO Project_SK (project_id, social_skill_id) VALUES (" + id + "," + Get_Id("select social_skill_id from Social_Skill where name LIKE '" + sk3 + "'") + ");";
            cmd = new SqlCommand(query, con);
            cmd.ExecuteNonQuery();
        }

    }
    public void InsertStudent(int id,string name, string mail, string phone, DateTime nasc, string country, int english_level, string institution, string area, int deg, string grade, int hs1, int hs2, int hs3, int ss1, int ss2, int ss3)
    {
        string query = "INSERT INTO Student (name,email,phone,date_birth,degree,institution_id,grade,english_value) VALUES ('" + name + "' ,'" + mail + "'," + phone + " ,'" + nasc.ToString("yyyy-MM-dd") + "' ," + deg + " ," + Get_Id("select institution_id from Institution where name LIKE '" + institution + "'") + "," + grade + "," + english_level +");";
        SqlCommand cmd = new SqlCommand(query, con);
        cmd.ExecuteNonQuery();

        query = "INSERT INTO Student_Project (student_id,project_id) VALUES (" + Get_Id("select max(student_id) from Student") + "," + id + ")";
        cmd = new SqlCommand(query, con);
        cmd.ExecuteNonQuery();

        query = "INSERT INTO Student_Area (student_id,area_id) VALUES (" + Get_Id("select max(student_id) from Student") + "," + Get_Id("select area_id from Area where name LIKE '" + area + "'") + ")";
        cmd = new SqlCommand(query, con);
        cmd.ExecuteNonQuery();

        int count = 0, tech1=0, tech2=0, tech3=0, sk1=0, sk2=0, sk3=0;
        query = "select PT.tech_id from Project_Tech PT where PT.project_id = '" + id + "'";
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            count++;
            switch (count)
            {
                case 0:
                    break;
                case 1:
                    tech1 = dr.GetInt32(0);
                    break;
                case 2:
                    tech2 = dr.GetInt32(0);
                    break;
                case 3:
                   tech3= dr.GetInt32(0);
                    break;
            }
        }
        dr.Close();
        dr.Dispose();

        for(int i = 0; i <= count; i++)
        {

       
            switch (i)
            {
                case 0:
                    break;
                case 1:
                    query = "INSERT INTO Student_Tech (student_id,tech_id,value) VALUES (" + Get_Id("select max(student_id) from Student") + "," + tech1 + "," + hs1 + ")";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    break;
                case 2:
                    query = "INSERT INTO Student_Tech (student_id,tech_id,value) VALUES (" + Get_Id("select max(student_id) from Student") + "," + tech2 + "," + hs2 + ")";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    break;
                case 3:
                    query = "INSERT INTO Student_Tech (student_id,tech_id,value) VALUES (" + Get_Id("select max(student_id) from Student") + "," + tech3 + "," + hs3 + ")";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    break;
            }
        }
        count = 0;
        query = "select PS.social_skill_id from Project_SK PS where PS.project_id = '" + id + "'";
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();
        while (dr.Read())
        {
            count++;

            switch (count)
            {
                case 0:
                    break;
                case 1:
                    sk1 = dr.GetInt32(0);
                    break;
                case 2:
                    sk2 = dr.GetInt32(0);
                    break;
                case 3:
                    sk3 = dr.GetInt32(0);
                    break;
            }
        }
        dr.Close();
        dr.Dispose();
        for (int i = 0; i <= count; i++)
        {
            switch (i)
            {
                case 0:
                    break;
                case 1:
                    query = "INSERT INTO Student_SK (student_id,social_skill_id,value) VALUES (" + Get_Id("select max(student_id) from Student") + "," + sk1 + "," + ss1 + ")";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    break;
                case 2:
                    query = "INSERT INTO Student_SK (student_id,social_skill_id,value) VALUES (" + Get_Id("select max(student_id) from Student") + "," + sk2 + "," + ss2 + ")";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    break;
                case 3:
                    query = "INSERT INTO Student_SK (student_id,social_skill_id,value) VALUES (" + Get_Id("select max(student_id) from Student") + "," + sk3 + "," + ss3 + ")";
                    cmd = new SqlCommand(query, con);
                    cmd.ExecuteNonQuery();
                    break;
            }
        }

    }



        public int Get_Id(string query)
    {
        int id = 0;
        cmd = new SqlCommand(query, con);
        dr = cmd.ExecuteReader();

        if (dr.Read())
        {
            id=  dr.GetInt32(0);
        }
        dr.Close();
        dr.Dispose();

        return id;
    }


}
