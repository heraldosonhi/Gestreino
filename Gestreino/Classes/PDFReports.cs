using Gestreino.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.Entity;
using DocumentFormat.OpenXml.EMMA;

namespace Gestreino.Classes
{
    public class PDFReports
    {
        public static string BodyMassPrintReport(int? Id,List<GT_Treino> treino, string path, string logo)
        {
            string Html = null;

            using (GESTREINO_Entities databaseManager = new GESTREINO_Entities())
            {

                //var treino = databaseManager.GT_Treino.Where(x => x.ID == Id).ToList();
                var tipotreinoID = treino.First().GT_TipoTreino_ID;
                var treinosp = databaseManager.SP_GT_ENT_TREINO(Id, null, tipotreinoID, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "R").ToList();
                var pesId = treinosp.Select(x => x.pes_id).FirstOrDefault();
                var caract = databaseManager.PES_PESSOAS_CARACT.Where(x => x.PES_PESSOAS_ID == pesId).ToList();
                var DuracaoPlanoId = caract.Select(x => x.GT_DuracaoPlano_ID).FirstOrDefault();
                var DuracaoPlano = databaseManager.GT_DuracaoPlano.Where(x => x.ID == DuracaoPlanoId).Select(x => x.DURACAO).FirstOrDefault() ;
                var exercicio = databaseManager.GT_ExercicioTreino.Where(x=>x.GT_Treino_ID == Id).ToList();
                var exIds = exercicio.Select(x=>x.GT_Exercicio_ID).ToList();
                var exSeries = exercicio.Select(x => x.GT_Series_ID).ToList();
                var exRepeticoes = exercicio.Select(x => x.GT_Repeticoes_ID).ToList();
                var exDescanso = exercicio.Select(x => x.GT_TempoDescanso_ID).ToList();
                var exCarga = exercicio.Select(x => x.GT_Carga_ID).ToList();

                //FR e OB
                List<Tuple<int, string>> FR = new List<Tuple<int, string>>();
                FR.Add(new Tuple<int, string>(1, "HT"));
                FR.Add(new Tuple<int, string>(2, "TB"));
                FR.Add(new Tuple<int, string>(3, "HL"));
                FR.Add(new Tuple<int, string>(4, "OB"));
                FR.Add(new Tuple<int, string>(5, "DB"));
                FR.Add(new Tuple<int, string>(6, "IN"));
                FR.Add(new Tuple<int, string>(7, "HE"));
                FR.Add(new Tuple<int, string>(8, "EC"));
                FR.Add(new Tuple<int, string>(9, "OT"));

                List<Tuple<int, string>> OB = new List<Tuple<int, string>>();
                OB.Add(new Tuple<int, string>(1, "AC"));
                OB.Add(new Tuple<int, string>(2, "CP"));
                OB.Add(new Tuple<int, string>(3, "PI"));
                OB.Add(new Tuple<int, string>(4, "TP"));
                OB.Add(new Tuple<int, string>(5, "AM"));
                OB.Add(new Tuple<int, string>(6, "BE"));
                OB.Add(new Tuple<int, string>(7, "TO"));
                OB.Add(new Tuple<int, string>(8, "OT"));

                var c = caract.First();
                var fr = new List<string>();
                if (c.FR_HT == true) fr.Add(FR.Where(x => x.Item1 ==1).Select(x=>x.Item2).FirstOrDefault());
                if (c.FR_TB == true) fr.Add(FR.Where(x => x.Item1 == 2).Select(x => x.Item2).FirstOrDefault());
                if (c.FR_HL == true) fr.Add(FR.Where(x => x.Item1 == 3).Select(x => x.Item2).FirstOrDefault());
                if (c.FR_OB == true) fr.Add(FR.Where(x => x.Item1 == 4).Select(x => x.Item2).FirstOrDefault());
                if (c.FR_DB == true) fr.Add(FR.Where(x => x.Item1 == 5).Select(x => x.Item2).FirstOrDefault());
                if (c.FR_IN == true) fr.Add(FR.Where(x => x.Item1 == 6).Select(x => x.Item2).FirstOrDefault());
                if (c.FR_HE == true) fr.Add(FR.Where(x => x.Item1 == 7).Select(x => x.Item2).FirstOrDefault());
                if (c.FR_EC == true) fr.Add(FR.Where(x => x.Item1 == 8).Select(x => x.Item2).FirstOrDefault());
                if (c.FR_OT == true) fr.Add(FR.Where(x => x.Item1 == 9).Select(x => x.Item2).FirstOrDefault());

                var o = caract.First();
                var ob = new List<string>();
                if (o.OB_AC == true) ob.Add(OB.Where(x => x.Item1 == 1).Select(x => x.Item2).FirstOrDefault());
                if (o.OB_CP == true) ob.Add(OB.Where(x => x.Item1 == 2).Select(x => x.Item2).FirstOrDefault());
                if (o.OB_PI == true) ob.Add(OB.Where(x => x.Item1 == 3).Select(x => x.Item2).FirstOrDefault());
                if (o.OB_TP == true) ob.Add(OB.Where(x => x.Item1 == 4).Select(x => x.Item2).FirstOrDefault());
                if (o.OB_AM == true) ob.Add(OB.Where(x => x.Item1 == 5).Select(x => x.Item2).FirstOrDefault());
                if (o.OB_BE == true) ob.Add(OB.Where(x => x.Item1 == 6).Select(x => x.Item2).FirstOrDefault());
                if (o.OB_TO == true) ob.Add(OB.Where(x => x.Item1 == 7).Select(x => x.Item2).FirstOrDefault());
                if (o.OB_OT == true) ob.Add(OB.Where(x => x.Item1 == 8).Select(x => x.Item2).FirstOrDefault());

                var images = (from j1 in databaseManager.GT_Exercicio_ARQUIVOS
                                               join j2 in databaseManager.GRL_ARQUIVOS on j1.ARQUIVOS_ID equals j2.ID
                                               where exIds.Contains(j1.GT_Exercicio_ID)
                                               select new { j1.ID, j1.GT_Exercicio_ID,j2.CAMINHO_URL }).ToList();


                var ename = databaseManager.GT_Exercicio.Where(x => exIds.Contains(x.ID)).ToList();
                var eseries = databaseManager.GT_Series.Where(x => exSeries.Contains(x.ID)).ToList();
                var erepeticoes = databaseManager.GT_Repeticoes.Where(x => exRepeticoes.Contains(x.ID)).ToList();
                var edescanso = databaseManager.GT_TempoDescanso.Where(x => exDescanso.Contains(x.ID)).ToList();
                var ecargas = databaseManager.GT_Carga.Where(x => exCarga.Contains(x.ID)).ToList();

                var tnome = treinosp.First().NOME_PROPIO + " " + treinosp.First().APELIDO;

                //var INST = (from j1 in databaseManager.INST_APLICACAO join j2 in databaseManager.INST_APLICACAO_ENDERECOS on j1.ID equals j2.INST_APLICACAO_ID join j3 in databaseManager.INST_APLICACAO_CONTACTOS on j1.ID equals j3.INST_APLICACAO_ID where j1.ID == Classes.Configs.INST_INSTITUICAO_ID select new { j2.MORADA, j3.TELEFONE, j3.TELEFONE_ALTERNATIVO, j3.EMAIL }).ToList();
                Html += "<html>";
                Html += "<link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css' rel='stylesheet' integrity='sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH' crossorigin='anonymous'>";
                Html += "<style> .footertext{font-size:9px;font-weight:900;} .p4{padding:.4rem;margin-bottom:0;margin-top:0} .tabletext{display: block;transform: rotate(90deg);padding:3rem;position:relative;width:inherit;bottom:.1rem}   .r-table td,.r-table th{padding:4px 5px!important;text-align:center}.r-table th span{padding: 0rem;}.r-table td,.r-table th,.text-center{text-align:center}@page{margin:1.2cm 1.2cm .4cm 1cm}body{font-family:Arial,Helvetica,sans-serif;font-size:14px;}.border{border:1px solid #000}.bg-ddd,.r-table td,.r-table th{border:1px solid #333}ul{padding:0;list-style:none}li{margin-bottom:10px}.d-flex{display:flex}.j-content-space-between{justify-content:space-between}.bg-ddd{background:#ddd}.text-md{font-size:12px}.main-header .header-img{text-align:center;padding-bottom:20px;border-bottom:2px solid #000}.main-content .section{margin-bottom:5px}.main-content .section-header{padding:2px 4px;font-weight:bolder;background-color:#e4e4e4;border:1px solid #000;margin-bottom:8px}.document-title{margin:10px auto}.section-content{padding:auto 15px}.main-footer .candidato p:first-child{width:7%}.main-footer .candidato{margin-bottom:0}.main-footer .candidato-obs{margin-top:0}.main-footer .candidato p:last-child{width:93%;padding:2px;border-bottom:1px solid #000}strong{font-weight:bolder}.item-tb{padding-bottom:5px}.item-l,.item-r{width:50%}.item-doc{width:33.333%}.p-y-1{padding:2px auto}table{width:100%}.main-header-title .entity{padding-bottom:2px}.main-header-title .country{letter-spacing:10px}.main-footer{width:100%;height:200px;position:absolute;z-index:100;bottom:0}.main-content{padding-bottom:190px}.r-table{}.r-table th{background-color:#ddd;height:143px}.r-table td:nth-of-type(3),.r-table th:nth-of-type(3){border:0!important;background-color:#fff}.text-left{text-align:left!important}.mt-1{margin-top:.2rem}</style>";
                Html += "<body>";
               
                Html += "<div class=bg-ddd><div class=row><div class=col-md-12>";
                Html += "<h1 class=\"p4\" style=\"font-weight:900;\">Ginásio Gestreino</h1></div></div><table><tr><td style=width:250px><h3 class=\"p4\" style=\"font-weight:900;\">Número: <small>"+treinosp.First().NUMERO+"</small></h3><td><h3 style=\"font-weight:900;\">Nome: <small>"+ tnome + "</small></h3></table></div><div class=\"bg-ddd mt-1\"><table><tr><td style=width:250px;padding: 0 !important><h3 class=\"p4\" style=\"font-weight:900;margin-bottom:0;margin-top:0\">Data de Início: <small>"+treinosp.First().DATA_INICIO+ "</small></h3><td><h3 style=\"font-weight:900;margin-bottom:0;margin-top:0;padding: 0 !important\">Factores de risco: <small>" + string.Join(", ", fr) + "</small></h3><tr><td style=width:250px><h3 class=\"p4\" style=\"font-weight:900;margin-bottom:0;margin-top:0\">Duração do plano: <small>"+ DuracaoPlano + "</small></h3><td><h3 style=\"font-weight:900;margin-bottom:0;margin-top:0\">Objectivos: <small>"+ string.Join(", ", ob) + "</small></h3><tr><td style=width:250px><h3 class=\"p4\" style=\"font-weight:900;\">Protocolo: <small>"+ caract.FirstOrDefault().OBSERVACOES + "</small></h3></table></div>";
                Html += "<div class=\"bg-ddd mt-1\"><div class=row><div class=col-md-12><center><h2 style=\"font-weight:900;margin-bottom:0\" >TREINO MUSCULAÇÃO</h1></center></div></div></div>";
                Html += "<div class=\"mt-1\"  style=\"height:420px\"><div class=row><div class=col-md-12><table class=r-table><thead><tr><th style=width:33px><span class=tabletext>Altura&nbsp;Banco</span><th style=width:33px><span class=tabletext>Inclinação</span><th style=width:33px><th style=width:123px colspan=3>Exercício<th style=width:33px><span class=tabletext>Alongamento</span><th style=width:83px>Séries<th style=width:100px>Descanso<th style=width:100px>Carga<th style=width:33px><span class=tabletext>Repetições</span><th style=width:33px><span class=tabletext>Ajust.&nbsp;Carga</span>";
                Html += "<tbody>";

                int t = 0;
                int i = 0;
                foreach (var ex in exercicio)
                {
                    t++;
                    var enome = ename.Where(x => x.ID == ex.GT_Exercicio_ID).Select(x => x.NOME).FirstOrDefault();
                    var eserie = eseries.Where(x => x.ID == ex.GT_Series_ID).Select(x => x.SERIES).FirstOrDefault();
                    var erepeticao = erepeticoes.Where(x => x.ID == ex.GT_Repeticoes_ID).Select(x => x.REPETICOES).FirstOrDefault();
                    var edescansos= edescanso.Where(x => x.ID == ex.GT_TempoDescanso_ID).Select(x => x.TEMPO_DESCANSO).FirstOrDefault();
                    var ecarga = ecargas.Where(x => x.ID == ex.GT_Carga_ID).Select(x => x.CARGA).FirstOrDefault();
                    var carga = ex.CARGA_USADA!=null? Math.Round((ex.CARGA_USADA.Value * ecarga) / 100):0;

                    Html += "<tr>";
                    Html += "<td class=bg-ddd></td>";
                    Html += "<td class=bg-ddd></td>";
                    Html += "<td><image width=\"14px\" src=\"https://www.svgrepo.com/show/188245/pointing-right-finger.svg\"></image></td>";
                    Html += "<td>"+t+".</td>";
                    Html += "<td class=bg-ddd>"+(ex.CARGA_USADA??0).ToString("G29")+"</td>";
                    Html += "<td class=text-left>"+enome+"</td>";
                    Html += "<td class=bg-ddd>0</td>";
                    Html += "<td>"+eserie+"</td>";
                    Html += "<td>"+ edescansos + "</td>";
                    Html += "<td>"+ carga + " kg</td>";
                    Html += "<td>"+ erepeticao + "</td>";
                    Html += "<td></td>";
                    Html += "</tr>";
                }

                Html += "</tbody></table></div></div></div>";
                Html += "<h1>&nbsp;</h1>";
                Html += "<table style=width:758px>";

                if (exercicio.Count() <= 6)
                {
                    Html += "<tr>";

                    foreach (var ex in exercicio)
                    {
                        i++;
                        var img = images.Where(x => x.GT_Exercicio_ID == ex.GT_Exercicio_ID).OrderByDescending(x => x.ID).Select(x => x.CAMINHO_URL).FirstOrDefault();
                        path += img.Replace("/", @"\");

                        Html += "<td><span style=margin-left:45px>" + i + "</span><br><img src=\""+path+"\" style=\"border:1px solid #333;width:100px\"></td>";
                    }
                    Html += "</tr>";
                }
                if (exercicio.Count() > 6)
                {
                    var skip = 0;
                    for (int y=1; y<=2;y++)
                    {
                            Html += "<tr>";
                            foreach (var ex in exercicio.Skip(skip).Take(6))
                            {
                                i++;
                                var img = images.Where(x => x.GT_Exercicio_ID == ex.GT_Exercicio_ID).OrderByDescending(x => x.ID).Select(x => x.CAMINHO_URL).FirstOrDefault();
                                path += img.Replace("/", @"\");

                                Html += "<td><span style=margin-left:45px>" + i + "</span><br><img src=\""+path+"\" style=\"border:1px solid #333;width:100px\"></td>";
                            }
                            Html += "</tr>";

                        skip = 6;
                    }
                }

                Html += "</table>";
                Html+= "<div class=\"bg-ddd mt-1\"><h5 class=\"p4\" style=\"font-weight:900;margin-bottom:0;font-size:13px;\"> Regras da sala de exercício:</h5><table class=\"table footertext\" style=\"margin-bottom:0\"><tr><td class=col-md-6>1. Faça sempre o aquecimento com exercícios de alongamentos<br>2. Utilize toalha para evitar o contacto directo com os equipamentos<br>3. Utilize ténis apropriados para a prática da actividade<br/>Obrigado pela sua compreensão<td class=col-md-6>4. Beba água antes, durante e após cada sessão<br>5. Não é permitido levar telemóveis e outros objectos para a sala de exercício<br>6. Cumpra com a prescrição dos instrutores e esclareça todas as dúvidas</table></div>";
                
                //Html += "   <div style=\"border-top: 2px solid black; padding-top: 2px; margin-top: 12px\">";
                //Html += "   </div>";
                //Html += "   <p class=\"text-center\" style=\"margin: 0; color: red\">";
                //Html += "       <small>";
                //Html += "           " + Configs.INST_INSTITUICAO_NOME + " - " + Configs.INST_INSTITUICAO_ENDERECO + " <br>";
                //Html += "           Tel. " + INST[0].TELEFONE + " / " + INST[0].TELEFONE_ALTERNATIVO + " Email: " + INST[0].EMAIL + "";
                //Html += "       </small>";
                //Html += "   </p>";
                //Html += " </footer>";
                //Html += " </body>";
                //Html += " </html>";
            }
            return Html;
        }
        public static string CardioPrintReport(int? Id, List<GT_Treino> treino, string path, string logo)
        {
            string Html = null;

            using (GESTREINO_Entities databaseManager = new GESTREINO_Entities())
            {

                //var treino = treino;// databaseManager.GT_Treino.Where(x => x.ID == Id).ToList();
                var tipotreinoID = treino.First().GT_TipoTreino_ID;
                var treinosp = databaseManager.SP_GT_ENT_TREINO(Id, null, tipotreinoID, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "R").ToList();
                var pesId = treinosp.Select(x => x.pes_id).FirstOrDefault();
                var caract = databaseManager.PES_PESSOAS_CARACT.Where(x => x.PES_PESSOAS_ID == pesId).ToList();
               var TreinoPessoa = databaseManager.GT_TreinosPessoa.Where(x => x.GT_Treino_ID == Id);
                var exercicio = databaseManager.GT_ExercicioTreinoCardio.Where(x => x.GT_Treino_ID == Id).ToList();
                var exIds = exercicio.Select(x => x.GT_Exercicio_ID).ToList();
              
                var DuracaoIds = exercicio.Select(x => x.GT_DuracaoTreinoCardio_ID);
                var DuracaoPlano = databaseManager.GT_DuracaoTreinoCardio.Where(x => DuracaoIds.Contains(x.ID)).ToList();

                var Age = 0;
                var dob = databaseManager.PES_PESSOAS.Where(x => x.ID == pesId).Select(x => x.DATA_NASCIMENTO).FirstOrDefault();
                if (dob != null)
                    Age = Converters.CalculateAge(dob.Value);

                var Peso = (caract.Select(x => x.PESO).FirstOrDefault() ?? 0).ToString("G29");
                var Vo2 = (caract.Select(x => x.VO2).FirstOrDefault() ?? 0).ToString("G29");
                var Observacoes = TreinoPessoa.Select(x => x.OBSERVACOES).FirstOrDefault();

                var ename = databaseManager.GT_Exercicio.Where(x => exIds.Contains(x.ID)).ToList();
                var tnome = treinosp.First().NOME_PROPIO + " " + treinosp.First().APELIDO;
                var periodizacao = treino.Select(x => x.PERIODIZACAO).FirstOrDefault() == 1 ? "Semana" : "Aula";

                //var INST = (from j1 in databaseManager.INST_APLICACAO join j2 in databaseManager.INST_APLICACAO_ENDERECOS on j1.ID equals j2.INST_APLICACAO_ID join j3 in databaseManager.INST_APLICACAO_CONTACTOS on j1.ID equals j3.INST_APLICACAO_ID where j1.ID == Classes.Configs.INST_INSTITUICAO_ID select new { j2.MORADA, j3.TELEFONE, j3.TELEFONE_ALTERNATIVO, j3.EMAIL }).ToList();
                Html += "<html>";
                Html += "<link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css' rel='stylesheet' integrity='sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH' crossorigin='anonymous'>";
                Html += "<style>th, td {font-size: 10px;} .footertext{font-size:9px;font-weight:900;} .p4{padding:.4rem;margin-bottom:0;margin-top:0} .tabletext{display: block;transform: rotate(90deg);padding:0;position:relative;width:15px;}   .r-table td,.r-table th{padding:3px 5px !important;text-align:center}.r-table th span{padding: 0rem;}.r-table td,.r-table th,.text-center{text-align:center}@page{margin:1.2cm 1.2cm .4cm 1cm}body{font-family:Arial,Helvetica,sans-serif;font-size:14px;}.border{border:1px solid #000}.bg-ddd,.r-table td,.r-table th{border:1px solid #333}ul{padding:0;list-style:none}li{margin-bottom:10px}.d-flex{display:flex}.j-content-space-between{justify-content:space-between}.bg-ddd{background:#ddd}.text-md{font-size:12px}.main-header .header-img{text-align:center;padding-bottom:20px;border-bottom:2px solid #000}.main-content .section{margin-bottom:5px}.main-content .section-header{padding:2px 4px;font-weight:bolder;background-color:#e4e4e4;border:1px solid #000;margin-bottom:8px}.document-title{margin:10px auto}.section-content{padding:auto 15px}.main-footer .candidato p:first-child{width:7%}.main-footer .candidato{margin-bottom:0}.main-footer .candidato-obs{margin-top:0}.main-footer .candidato p:last-child{width:93%;padding:2px;border-bottom:1px solid #000}strong{font-weight:bolder}.item-tb{padding-bottom:5px}.item-l,.item-r{width:50%}.item-doc{width:33.333%}.p-y-1{padding:2px auto}table{width:100%}.main-header-title .entity{padding-bottom:2px}.main-header-title .country{letter-spacing:10px}.main-footer{width:100%;height:200px;position:absolute;z-index:100;bottom:0}.main-content{padding-bottom:190px}.r-table{width:1039px}.r-table th{background-color:#ddd;}.r-table td:nth-of-type(3),.r-table th:nth-of-type(3){}.text-left{text-align:left!important}.mt-1{margin-top:.6rem}</style>";
                Html += "<body>";
                Html += "<div class=\"mt-1 bg-ddd\"><center><h1 style=\"font-weight:900;margin-bottom:0\">TREINO CARDIOVASCULAR</h1></center></div>";
                Html += "<div class=\"bg-ddd mt-1\"><table><tr cl><td style=width:250px><h3 class=p4 style=\"font-weight:900;font-size:14px;\">Nome: <small>"+ tnome + "</small></h3><td><h3 class=p4 style=\"font-weight:900;font-size:14px;\">Idade: <small>"+ Age + "</small></h3><td><h3 class=p4 style=\"font-weight:900;font-size:14px;\">Peso: <small>"+Peso+ " Kg</small></h3><td><h3 class=p4 style=\"font-weight:900;font-size:14px;\">Massa gorda: <small>%</small></h3><td><h3 class=p4 style=\"font-weight:900;font-size:14px;\">VO2: <small>" + Vo2 + " ml/kg/min</small></h3></table></div>";
                Html += "<div class=mt-1><table class=r-table><thead><tr><th style=width:33px>PROGRAMA<th style=width:33px>"+ periodizacao.ToUpper()+"S".ToUpper() + "<th>"+ periodizacao + " 1<th>"+ periodizacao + " 2<th>"+ periodizacao + " 3<th>"+ periodizacao + " 4<th>"+ periodizacao + " 5<th>"+ periodizacao + " 6<th>"+ periodizacao + " 7<th>"+ periodizacao + " 8<th>"+ periodizacao + " 9<th>"+ periodizacao + " 10<th>"+ periodizacao + " 11<th>"+ periodizacao + " 12<tbody><tr><td style=border:0!important><td style=border:0!important><td style=border:0!important><img src=https://static.thenounproject.com/png/775310-200.png width=16><td style=border:0!important><img src=https://static.thenounproject.com/png/775310-200.png width=16><td style=border:0!important><img src=https://static.thenounproject.com/png/775310-200.png width=16><td style=border:0!important><img src=https://static.thenounproject.com/png/775310-200.png width=16><td style=border:0!important><img src=https://static.thenounproject.com/png/775310-200.png width=16><td style=border:0!important><img src=https://static.thenounproject.com/png/775310-200.png width=16><td style=border:0!important><img src=https://static.thenounproject.com/png/775310-200.png width=16><td style=border:0!important><img src=https://static.thenounproject.com/png/775310-200.png width=16><td style=border:0!important><img src=https://static.thenounproject.com/png/775310-200.png width=16><td style=border:0!important><img src=https://static.thenounproject.com/png/775310-200.png width=16><td style=border:0!important><img src=https://static.thenounproject.com/png/775310-200.png width=16><td style=border:0!important><img src=https://static.thenounproject.com/png/775310-200.png width=16>";

                int t = 0;
                int i = 0;
                foreach (var ex in exercicio)
                {
                    t++;
                    var enome = ename.Where(x => x.ID == ex.GT_Exercicio_ID).Select(x => x.NOME).FirstOrDefault();
                    var duracao = DuracaoPlano.Where(x => x.ID == ex.GT_DuracaoTreinoCardio_ID).Select(x => x.DURACAO).FirstOrDefault();

                    var strTemp = string.Empty;
                    var Vel = string.Empty;
                    var inclinacao = string.Empty;
                    var fc = ex.FC==null?"":ex.FC.ToString();
                    var nivel = ex.NIVEL==null?"" : (ex.NIVEL ?? 0).ToString("G29");

                    if (Configs.GT_EXERCISE_TYPE_CARDIO_INCLINACAO.Contains(ex.ID))
                    { strTemp = "Duração/Inclinação"; inclinacao = (ex.DISTANCIA ?? 0).ToString("G29"); }
                    else
                        strTemp = "Duração";


                    if (ex.ID == Configs.GT_EXERCISE_TYPE_CARDIO_INCLINACAO[0])//Se for a passadeira aparece a label Velocidade
                        Vel = "Velocidade";
                    else //Caso contrário mostra a label normal...
                        Vel = "Nível / Resist. (W)";

                    Html += "<tr><td style=border:0!important><table><tr><td rowspan=5 style=\"background:#666;color:#fff\"><span class=tabletext>" + enome + "</span><td>" + strTemp + "<tr><td>FC (Mín./Máx.) bpm<tr><td>" + Vel + "<tr><td class=\"bg-ddd\">Tempo/Distância(km)<tr><td class=\"bg-ddd\">Dispêndio</table>";
                    Html+="<td style=border:0!important><table class=intable><tr><td>"+ duracao + "' "+ inclinacao + "<tr><td>"+fc+"&nbsp;<tr><td>"+nivel+"&nbsp;<tr><td style=border:0!important><img src=https://www.svgrepo.com/show/188245/pointing-right-finger.svg width=14><tr><td style=border:0!important><img src=https://www.svgrepo.com/show/188245/pointing-right-finger.svg width=14></table><td style=border:0!important><table><tr><td>6<tr><td>67<tr><td>45<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>6<tr><td>67<tr><td>45<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>6<tr><td>67<tr><td>45<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>6<tr><td>67<tr><td>45<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>6<tr><td>67<tr><td>45<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>6<tr><td>67<tr><td>45<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>6<tr><td>67<tr><td>45<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>6<tr><td>67<tr><td>45<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>6<tr><td>67<tr><td>45<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>6<tr><td>67<tr><td>45<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>6<tr><td>67<tr><td>45<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>6<tr><td>67<tr><td>45<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table>";
                }
                Html += "</table>";
                Html += "<table class=r-table><tr><td style=\"border:0!important\"><td>1<td>2<td>3<td>4<td>5<td>6<td>7<td>8<td>9<td>10<td>11<td>12<td>13<td>14<td>15<td>16<td>17<td>18<td>19<td>20<td>21<td>22<td>23<td>24<td>25<td>26<td>27<td>28<td>29<td>30<td>31";

                int FirstMonth = TreinoPessoa.Select(x => x.DATA_INICIO).FirstOrDefault().Month;
                int iMonth = 0;
             
                    for (int intMes = 0; intMes < 4; intMes++)
                    {
                        //Se a soma for superior a 12 então é pq mudou-se de ano
                        iMonth = Convert.ToInt32(FirstMonth) + intMes;
                        if ((Convert.ToInt32(FirstMonth) + intMes) > 12)
                            iMonth -= 12;
                        string fullMonthName = new DateTime(2015, iMonth, 1).ToString("MMMM", CultureInfo.CreateSpecificCulture("pt")).ToTitleCase();
                        Html += "<tr><td>"+ fullMonthName + "<td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td><td>";
                }

                Html +="</table></div>";
                Html += "<div class=\"mt-1 bg-ddd\"><h3 style=\"font-weight:900;margin-bottom:0;font-size:11px;\">&nbsp;Observações: " + Observacoes + "</h3><p class=mt-1>&nbsp;</p></div>";
                
                //Html += "   <div style=\"border-top: 2px solid black; padding-top: 2px; margin-top: 12px\">";
                //Html += "   </div>";
                //Html += "   <p class=\"text-center\" style=\"margin: 0; color: red\">";
                //Html += "       <small>";
                //Html += "           " + Configs.INST_INSTITUICAO_NOME + " - " + Configs.INST_INSTITUICAO_ENDERECO + " <br>";
                //Html += "           Tel. " + INST[0].TELEFONE + " / " + INST[0].TELEFONE_ALTERNATIVO + " Email: " + INST[0].EMAIL + "";
                //Html += "       </small>";
                //Html += "   </p>";
                //Html += " </footer>";
                //Html += " </body>";
                //Html += " </html>";
            }
            return Html;
        }

    }
}