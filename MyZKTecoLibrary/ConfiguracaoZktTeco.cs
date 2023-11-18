using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKSoftwareAPI;


namespace MyZKTecoLibrary
{
    public static class ConfiguracaoZktTeco
    {
        static ZKSoftware Dispositivo = new ZKSoftware(Modelo.X628C);
        public static bool Conectado { get; private set; }
        public static bool Conectar(string Ip)
        {
            try
            {
                if (Dispositivo.DispositivoConectar(Ip, 0, false))
                {

                    Dispositivo.DispositivoCambiarHoraAutomatico();
                    return  Conectado = true;
                   
                }   

                return false;
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }
        public static bool ConectarComBeep(string Ip)
        {
            try
            {
                if (Dispositivo.DispositivoConectar(Ip, 0, true))
                {
                    Dispositivo.DispositivoCambiarHoraAutomatico();
                    
                    return Conectado = true;
                }

                return false;
            }
            catch (Exception e)
            {

                throw e;
            }

        }
        public static void Desconectar()
        {
            try
            {
                
                Dispositivo.DispositivoDesconectar();
                
                Conectado = false;
                
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }
        public static bool Beep()
        {
            try
            {
                if (Conectado)
                {
                    return Dispositivo.Beep();
                }
                return false;
            }
            catch (Exception e)
            {

                throw e;
            }
        } 
        public static List<Marcacao> Marcacoes1()
        {
            try
            {
                Dispositivo.DispositivoObtenerRegistrosAsistencias();
                var disp = Dispositivo.ListaMarcajes;
                List<Marcacao> marcacao = new List<Marcacao>();

                foreach (var item in disp)
                {
                    marcacao.Add(new Marcacao
                    {
                        UserId = item.NumeroCredencial,
                        Ano = item.Anio,
                        Mes = item.Mes,
                        Dia = item.Dia,
                        Hora = item.Hora,
                        Minuto = item.Minuto,
                        Segundo = item.Segundo,

                        DataCompleta = DateTime.Parse(item.Dia + "/" + item.Mes + "/" + item.Anio),
                        HorarioCompleto = DateTime.Parse(item.Hora + ":" + item.Minuto + ":" + item.Segundo)
                    });
                }
                return marcacao.OrderByDescending(x=> x.DataCompleta ).ToList();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public static List<Marcacoes> Marcacoes2() 
        {
            try
            {
                Dispositivo.DispositivoObtenerRegistrosAsistencias();

                var qwery = from m in Marcacoes1().ToList()
                            join u in BuscarTodosUsers().ToList()
                            on m.UserId equals u.Id
                            select new
                            {
                                m.UserId,
                                u.Nome,
                                m.DataCompleta,
                                m.HorarioCompleto
                            };
                List<Marcacoes> marc = new List<Marcacoes>();
                foreach (var item in qwery)
                {
                    marc.Add(new Marcacoes
                    {
                        UserId = item.UserId,
                        UserName = item.Nome,
                        DataRegistada = item.HorarioCompleto

                    });
                }

                return marc;        

            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public static List<UserZk> BuscarTodosUsers()
        {
            try
            {
                List<UserZk> us = new List<UserZk>();
                if (Conectado)
                {
                    Dispositivo.UsuarioBuscarTodos(true);

                    foreach (var item in Dispositivo.ListaUsuarios)
                    {
                        us.Add(new UserZk
                        {
                            Id= item.NumeroCredencial,
                            Nome= item.Nombre
                        });
                    }
                   

                    return us;
                }

                return new List<UserZk>();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public static bool AdicionarUserNormal(string Nome, string ip)
        {
            try
            {
                if (Conectar(ip))
                {
                    int n = BuscarTodosUsers().Count + 1;
                    return Dispositivo.UsuarioAgregar(n, Nome, Permiso.UsuarioNormal, 0, "0000000");
                   
                }

                return false;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public static bool AdicionarUserNormalbeep(string Nome, string ip)
        {
            try
            {
                if (ConectarComBeep(ip))
                {
                    int n = BuscarTodosUsers().Count + 1;
                    Nome += n;
                    return Dispositivo.UsuarioAgregar(n, Nome, Permiso.UsuarioNormal, 0, "0000000");
                    
                }

                return false;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public static string AdicionarUserNormalbeep1(string Nome, string ip)
        {
            try
            {
                if (ConectarComBeep(ip))
                {
                    int n = BuscarTodosUsers().Count + 1;
                    Nome += n;
                     Dispositivo.UsuarioAgregar(n, Nome, Permiso.UsuarioNormal, 0, "0000000");
                    return Nome;

                }

                return null;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        public static bool EliminarUser(int id)
        {
            try
            {
                if (Conectado)
                {
                    int n = BuscarTodosUsers().Count + 1;

                 return   Dispositivo.UsuarioBorrar(id);
                }

                return false; 
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }

}

