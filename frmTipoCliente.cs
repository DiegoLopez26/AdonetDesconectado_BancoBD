using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AdoNetDesconectado
{
    public partial class frmTipoCliente : Form
    {
        string cadenaConexion = @"Server=LAPTOP-MKA1MDEC; DataBase=BancoBD; Integrated Security=true";
        SqlDataAdapter adaptador;
        SqlConnection conexion;
        DataSet datos;
        
        public frmTipoCliente()
        {
            InitializeComponent();
            dgvDatos.AutoGenerateColumns = false;

            // Creamos la instancia de la Conexion
            conexion = new SqlConnection(cadenaConexion);

            // Creamos la instancia del Adaptador
            adaptador = new SqlDataAdapter();

            // Creamos la instancia del DataSet
            datos = new DataSet();

            // Configurar métodos del adaptador
            adaptador.SelectCommand = new SqlCommand("SELECT * FROM TipoCliente", conexion);
            //Configuración de insert command
            adaptador.InsertCommand = new SqlCommand("INSERT INTO TipoCliente(Nombre) VALUES(@nombre)", conexion);
            adaptador.InsertCommand.Parameters.Add("@nombre", SqlDbType.VarChar, 20, "Nombre");
            //Configuracion del metodo editar
            adaptador.UpdateCommand = new SqlCommand("Update TipoCliente set Nombre = @nombre Where ID = @id", conexion);
            adaptador.UpdateCommand.Parameters.Add("@nombre ", SqlDbType.VarChar, 20, "Nombre");
            adaptador.UpdateCommand.Parameters.Add("@id", SqlDbType.Int, 1, "ID");
            //
            adaptador.DeleteCommand = new SqlCommand("Delete from TipoCliente where ID=@id");
            adaptador.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 1, "ID");

        }

        private void cargarFormulario(object sender, EventArgs e)
        {
            mostrarDatos();
        }

        private void mostrarDatos()
        {
            // Llenar datos al DataSet (DataTable TipoCliente)
            adaptador.Fill(datos, "TipoCliente");

            //Enlazar datos al DataGridView
            dgvDatos.DataSource = datos.Tables["TipoCliente"];

        }

        private void nuevoRegistro(object sender, EventArgs e)
        {
            var frm = new frmTipoClienteEdit();
            if(frm.ShowDialog() == DialogResult.OK)
            {
                var nuevaFila = datos.Tables["TipoCliente"].NewRow();
                nuevaFila["Nombre"] = frm.Controls["txtNombre"].Text;

                datos.Tables["TipoCliente"].Rows.Add(nuevaFila);
               
            }
        }

        private void editarRegistro(object sender, EventArgs e)
        {
            var filaActual = dgvDatos.CurrentRow;
            if(filaActual != null)
            {
                var ID = filaActual.Cells[0].Value.ToString();
                DataRow fila = datos.Tables["TipoCliente"].Select($"ID={ID}").FirstOrDefault();

                var frm = new frmTipoClienteEdit(fila);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    fila["Nombre"] = frm.Controls["txtNombre"].Text;
                   
                }
            } 
        }

        private void tsbSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void actualizarBD(object sender, EventArgs e)
        {
            //Actualiza la informacion 
            adaptador.Update(datos.Tables["TipoCliente"]);
            //Limpiar la tabla para cuando se cargue nuevamente la tabla no duplique 
            datos.Tables["TipoCliente"].Clear();
            mostrarDatos();
        }

        private void tsbEliminar_Click(object sender, EventArgs e)
        {
            var filaActual = dgvDatos.CurrentRow;
            if (filaActual != null)
            {
                var ID = filaActual.Cells[0].Value.ToString();
                DataRow fila = datos.Tables["TipoCliente"].Select($"ID={ID}").FirstOrDefault();
                var frm = new frmTipoClienteEdit(fila);

            }

        }
    }
}
