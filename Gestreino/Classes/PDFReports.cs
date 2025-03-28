using Gestreino.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.Entity;
using DocumentFormat.OpenXml.EMMA;
using System.Data.OleDb;

namespace Gestreino.Classes
{
    public class PDFReports
    {
        public static string BodyMassPrintReport(int? Id,List<GT_Treino> treino, string path, string logo)
        {
            string Html = null;

            using (GESTREINO_Entities databaseManager = new GESTREINO_Entities())
            {
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
                        img =path+img.Replace("/", @"\");

                        Html += "<td><span style=margin-left:45px>" + i + "</span><br><img src=\""+img+"\" style=\"border:1px solid #333;width:100px\"></td>";
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
            }
            return Html;
        }
        public static string CardioPrintReport(int? Id, List<GT_Treino> treino, string path, string logo)
        {
            string Html = null;

            using (GESTREINO_Entities databaseManager = new GESTREINO_Entities())
            {
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
                    var v12 = string.Empty;
                    var fc = ex.FC==null?"":ex.FC.ToString();
                    var nivel = ex.NIVEL==null?"" : (ex.NIVEL ?? 0).ToString("G29");

                    if (Configs.GT_EXERCISE_TYPE_CARDIO_INCLINACAO.Contains(ex.GT_Exercicio_ID))
                    { strTemp = "Duração/Inclinação"; inclinacao = (ex.DISTANCIA ?? 0).ToString("G29"); v12 = "|"; }
                    else
                        strTemp = "Duração";

                    if (ex.GT_Exercicio_ID == Configs.GT_EXERCISE_TYPE_CARDIO_INCLINACAO[0])//Se for a passadeira aparece a label Velocidade
                        Vel = "Velocidade";
                    else //Caso contrário mostra a label normal...
                        Vel = "Nível / Resist. (W)";

                    Html += "<tr><td style=border:0!important><table><tr><td rowspan=5 style=\"background:#666;color:#fff\"><span class=tabletext>" + enome + "</span><td>" + strTemp + "<tr><td>FC (Mín./Máx.) bpm<tr><td>" + Vel + "<tr><td class=\"bg-ddd\">Tempo/Distância(km)<tr><td class=\"bg-ddd\">Dispêndio</table>";
                    Html += "<td style=border:0!important><table class=intable><tr><td>" + duracao + "' " + inclinacao + "<tr><td>" + fc + "&nbsp;<tr><td>" + nivel + "&nbsp;<tr><td style=border:0!important><img src=https://www.svgrepo.com/show/188245/pointing-right-finger.svg width=14><tr><td style=border:0!important><img src=https://www.svgrepo.com/show/188245/pointing-right-finger.svg width=14></table>";
                    Html+="<td style=border:0!important><table><tr><td>"+ v12 + "&nbsp;<tr><td>|<tr><td>|<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>"+ v12 + "&nbsp;<tr><td>|<tr><td>|<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>"+ v12 + "&nbsp;<tr><td>|<tr><td>|<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>"+ v12 + "&nbsp;<tr><td>|<tr><td>|<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>"+ v12 + "&nbsp;<tr><td>|<tr><td>|<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>"+ v12 + "&nbsp;<tr><td>|<tr><td>|<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>"+ v12 + "&nbsp;<tr><td>|<tr><td>|<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>"+ v12 + "&nbsp;<tr><td>|<tr><td>|<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>"+ v12 + "&nbsp;<tr><td>|<tr><td>|<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>"+ v12 + "&nbsp;<tr><td>|<tr><td>|<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>"+ v12 + "&nbsp;<tr><td>|<tr><td>|<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table><td style=border:0!important><table><tr><td>"+ v12 + "&nbsp;<tr><td>|<tr><td>|<tr><td class=bg-ddd>&nbsp;</td></tr><tr><td class=bg-ddd>&nbsp;</td></tr></table>";
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
            }
            return Html;
        }





        public static string MainReportPrint(int? PEsId, string path, string logo)
        {
            string Html = null;

            using (GESTREINO_Entities databaseManager = new GESTREINO_Entities())
            {
               
              
                Html += "<html>";
                Html += "<link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css' rel='stylesheet' integrity='sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH' crossorigin='anonymous'>";
                Html += "<style>.footertext{font-size:9px;font-weight:900;} .p4{padding:.4rem;margin-bottom:0;margin-top:0} .tabletext{display: block;transform: rotate(90deg);padding:3rem;position:relative;width:inherit;bottom:.1rem}   .r-table td,.r-table th{padding:4px 5px!important;text-align:center}.r-table th span{padding: 0rem;}.r-table td,.r-table th,.text-center{text-align:center}@page{margin:1.7cm 1.7cm 1.7cm 1.7cm}body{font-family:Arial,Helvetica,sans-serif;font-size:14px;}.border{border:1px solid #000}.bg-ddd,.r-table td,.r-table th{border:1px solid #333}ul{padding:0;list-style:none}li{margin-bottom:10px}.d-flex{display:flex}.j-content-space-between{justify-content:space-between}.bg-ddd{background:#ddd}.text-md{font-size:12px}.main-header .header-img{text-align:center;padding-bottom:20px;border-bottom:2px solid #000}.main-content .section{margin-bottom:5px}.main-content .section-header{padding:2px 4px;font-weight:bolder;background-color:#e4e4e4;border:1px solid #000;margin-bottom:8px}.document-title{margin:10px auto}.section-content{padding:auto 15px}.main-footer .candidato p:first-child{width:7%}.main-footer .candidato{margin-bottom:0}.main-footer .candidato-obs{margin-top:0}.main-footer .candidato p:last-child{width:93%;padding:2px;border-bottom:1px solid #000}strong{font-weight:bolder}.item-tb{padding-bottom:5px}.item-l,.item-r{width:50%}.item-doc{width:33.333%}.p-y-1{padding:2px auto}table{width:100%}.main-header-title .entity{padding-bottom:2px}.main-header-title .country{letter-spacing:10px}.main-footer{width:100%;height:200px;position:absolute;z-index:100;bottom:0}.main-content{padding-bottom:190px}.r-table{}.r-table th{background-color:#ddd;height:143px}.r-table td:nth-of-type(3),.r-table th:nth-of-type(3){border:0!important;background-color:#fff}.text-left{text-align:left!important}.mt-1{margin-top:.2rem}</style>";
                Html += "<body>";
                
                Reports MODEL=new Reports();
                var Age = 0;
                var pes = databaseManager.PES_PESSOAS.Where(x => x.ID == PEsId);
                var dob = pes.Select(x => x.DATA_NASCIMENTO).FirstOrDefault();
                if (dob != null)
                    Age = Converters.CalculateAge(dob.Value);

                var caract = databaseManager.PES_PESSOAS_CARACT.Where(x => x.PES_PESSOAS_ID == PEsId).ToList();


                //pageone
                Html += "<div class=mt-1><center><h1><img src=https://gestreino.pt/Assets/images/icon.jpg></h1></center></div><p class=mt-1><div class=mt-1><center><h1>Bem vindo ao seu relatorio pessoal</h1></center></div><div class=mt-1><center><h1><img src=\"https://assets.discours.io/unsafe/1600x/production/image/c7717d80-90ee-11e8-acc9-1177899fe735.jpeg\" width=\"400px\"></h1></center></div><div><table><tr><td>Idade: 21<tr><td>Peso: 21<tr><td>Altura: 21</table></div>";
                Html += "<div style=\"page-break-before: always;\"></div>";
           

                var GTSocioId = databaseManager.GT_SOCIOS.Where(x => x.PES_PESSOAS_ID == PEsId).Select(x => x.ID).FirstOrDefault();

                var data1 = databaseManager.GT_RespAnsiedadeDepressao.Where(x => x.GT_SOCIOS_ID == GTSocioId).ToList();
                MODEL.lblDataAnsiedade = data1.Any() ? data1.OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;

                var data2 = databaseManager.GT_RespAutoConceito.Where(x => x.GT_SOCIOS_ID == GTSocioId).ToList();
                MODEL.lblDataAutoConceito = data2.Any() ? data2.OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;

                var data3 = databaseManager.GT_RespRisco.Where(x => x.GT_SOCIOS_ID == GTSocioId).ToList();
                MODEL.lblDataAutoRisco = data3.Any() ? data3.OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;

                var data4 = databaseManager.GT_RespProblemasSaude.Where(x => x.GT_SOCIOS_ID == GTSocioId).ToList();
                MODEL.lblDataHealth = data4.Any() ? data4.OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;

                var data5 = databaseManager.GT_RespFlexiTeste.Where(x => x.GT_SOCIOS_ID == GTSocioId).ToList();
                MODEL.lblDataFlexi = data5.Any() ? data5.OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;

                var data6 = databaseManager.GT_RespComposicao.Where(x => x.GT_SOCIOS_ID == GTSocioId).ToList();
                MODEL.lblDataComposicaoCorporal = data6.Any() ? data6.OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;

                var data7 = databaseManager.GT_RespAptidaoCardio.Where(x => x.GT_SOCIOS_ID == GTSocioId).ToList();
                MODEL.lblDataCardio = data7.Any() ? data7.OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;

                var data8 = databaseManager.GT_RespPessoaIdosa.Where(x => x.GT_SOCIOS_ID == GTSocioId).ToList();
                MODEL.lblDataSentarCadeira = data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 1).Any() ? data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 1).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblDataFlexaoAntebraco = data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 2).Any() ? data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 2).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblDataPeso = data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 3).Any() ? data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 3).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblDataLevantarCadeira = data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 4).Any() ? data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 4).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblDataAgilidade = data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 5).Any() ? data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 5).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblDataAlcancar = data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 6).Any() ? data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 6).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblData6Minutos = data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 7).Any() ? data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 7).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblDataStep = data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 8).Any() ? data8.Where(x => x.GT_TipoTestePessoaIdosa_ID == 8).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;

                var data9 = databaseManager.GT_RespForca.Where(x => x.GT_SOCIOS_ID == GTSocioId).ToList();
                MODEL.lblData1RMBraco = data9.Where(x => x.GT_TipoTesteForca_ID == 1).Any() ? data9.Where(x => x.GT_TipoTesteForca_ID == 1).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblData1RMPerna = data9.Where(x => x.GT_TipoTesteForca_ID == 2).Any() ? data9.Where(x => x.GT_TipoTesteForca_ID == 2).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblDataResistenciaMedia = data9.Where(x => x.GT_TipoTesteForca_ID == 3).Any() ? data9.Where(x => x.GT_TipoTesteForca_ID == 3).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblDataResistenciaSuperior = data9.Where(x => x.GT_TipoTesteForca_ID == 4).Any() ? data9.Where(x => x.GT_TipoTesteForca_ID == 4).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblDataVelocidadeLinear = data9.Where(x => x.GT_TipoTesteForca_ID == 5).Any() ? data9.Where(x => x.GT_TipoTesteForca_ID == 5).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblDataVelocidadeResistente = data9.Where(x => x.GT_TipoTesteForca_ID == 6).Any() ? data9.Where(x => x.GT_TipoTesteForca_ID == 6).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblDataAgilidade = data9.Where(x => x.GT_TipoTesteForca_ID == 7).Any() ? data9.Where(x => x.GT_TipoTesteForca_ID == 7).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblDataExplosivaH = data9.Where(x => x.GT_TipoTesteForca_ID == 8).Any() ? data9.Where(x => x.GT_TipoTesteForca_ID == 8).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                MODEL.lblDataExplosivaV = data9.Where(x => x.GT_TipoTesteForca_ID == 9).Any() ? data9.Where(x => x.GT_TipoTesteForca_ID == 9).OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;

                var data10 = databaseManager.GT_RespFuncional.Where(x => x.GT_SOCIOS_ID == GTSocioId).ToList();
                MODEL.lblDataFuncional = data10.Any() ? data10.OrderByDescending(x => x.DATA_INSERCAO).Select(x => x.DATA_INSERCAO).FirstOrDefault().ToString("dd/MM/yyyy hh:mm:ss") : string.Empty;
                
                string HtmlPdf = string.Empty;

                //Ansiedade e Depressão
                if (!string.IsNullOrEmpty(MODEL.lblDataAnsiedade))
                    HtmlPdf += AddResultDepressao(data1, pes.Select(x => x.NOME).FirstOrDefault());
                
                //Auto-Conceito
                if (!string.IsNullOrEmpty(MODEL.lblDataAutoConceito))
                    HtmlPdf+=AddResultAutoConceito(data2, pes.Select(x => x.NOME).FirstOrDefault());

                //Rsco Coronário
                if (!string.IsNullOrEmpty(MODEL.lblDataAutoRisco))
                    HtmlPdf += AddResultRiscoCoronario(data3,caract.Select(x=>x.IMC).FirstOrDefault());

                //Avaliação Funcional
                if (!string.IsNullOrEmpty(MODEL.lblDataFuncional))
                    HtmlPdf += AddResultFuncional(data10);

                //Problemas de Saúde
                if (!string.IsNullOrEmpty(MODEL.lblDataHealth))
                    HtmlPdf += AddResultSaude(data4);


                /*
                //Flexibildade
                if (!string.IsNullOrEmpty(MODEL.lblDataFlexi))
                    AddResultFlexibilidade(ref f, ref numAvaliacao, Connection);

                //Composição Corporal
                if (!string.IsNullOrEmpty(MODEL.lblDataComposicaoCorporal))
                    AddResultComposicao(ref f, ref numAvaliacao, Connection);

                //Aptidão Cardiorespiratória
                if (!string.IsNullOrEmpty(MODEL.lblDataCardio))
                    AddResultAptidao(ref f, ref numAvaliacao, Connection);

                //Pessoa Idosa - Levantar e Sentar na Cadeira
                if (!string.IsNullOrEmpty(MODEL.lblDataSentarCadeira))
                    AddResultPessoaIdosa(ref f, ref numAvaliacao, Connection, lblDataLevantar.Text, "Levantar e Sentar na Cadeira", "1");

                //Pessoa Idosa - Flexão do Antebraço
                if (!string.IsNullOrEmpty(MODEL.lblDataFlexaoAntebraco))
                    AddResultPessoaIdosa(ref f, ref numAvaliacao, Connection, lblDataFlexao.Text, "Flexão do Antebraço", "2");

                //Pessoa Idosa - Estatura e Peso
                if (!string.IsNullOrEmpty(MODEL.lblDataPeso))
                    AddResultPessoaIdosa(ref f, ref numAvaliacao, Connection, lblDataEstatura.Text, "Estatura e Peso", "3");

                //Pessoa Idosa - Sentar e Alcançar na Cadeira
                if (!string.IsNullOrEmpty(MODEL.lblDataLevantarCadeira))
                    AddResultPessoaIdosa(ref f, ref numAvaliacao, Connection, lblDataSentar.Text, "Sentar e Alcançar na Cadeira", "4");

                //Pessoa Idosa - Agilidade, Velocidade e Equilíbrio
                if (!string.IsNullOrEmpty(MODEL.lblDataAgilidade))
                    AddResultPessoaIdosa(ref f, ref numAvaliacao, Connection, lblDataAgi.Text, "Agilidade, Velocidade e Equilíbrio", "5");

                //Pessoa Idosa - Alcançar Atrás das Costas
                if (!string.IsNullOrEmpty(MODEL.lblDataAlcancar))
                    AddResultPessoaIdosa(ref f, ref numAvaliacao, Connection, lblDataAlcancar.Text, "Alcançar Atrás das Costas", "6");

                //Pessoa Idosa - 6 Minutos - Andar
                if (!string.IsNullOrEmpty(MODEL.lblData6Minutos))
                    AddResultPessoaIdosa(ref f, ref numAvaliacao, Connection, lblData6Andar.Text, "6 Minutos - Andar", "7");

                //Pessoa Idosa - 2 Minutos a Subir o Step
                if (!string.IsNullOrEmpty(MODEL.lblDataStep))
                    AddResultPessoaIdosa(ref f, ref numAvaliacao, Connection, lblData2Subir.Text, "2 Minutos a Subir o Step", "8");

                //Força Braços
                if (!string.IsNullOrEmpty(MODEL.lblData1RMBraco))
                    AddResultForcaBracos(ref f, ref numAvaliacao, Connection);

                //Força Pernas
                if (!string.IsNullOrEmpty(MODEL.lblData1RMPerna))
                    AddResultForcaPernas(ref f, ref numAvaliacao, Connection);

                //Resistência Média / Abdominais
                if (!string.IsNullOrEmpty(MODEL.lblDataResistenciaMedia))
                    AddResultForcaAbdominais(ref f, ref numAvaliacao, Connection);

                //RResistência Superior / Flexões Braços
                if (!string.IsNullOrEmpty(MODEL.lblDataResistenciaSuperior))
                    AddResultForcaFlexoes(ref f, ref numAvaliacao, Connection);


                //Velocidade Linear
                if (!string.IsNullOrEmpty(MODEL.lblDataVelocidadeLinear))
                    AddResultForcaVLinear(ref f, ref numAvaliacao, Connection);

                //Velocidade Resistente
                if (!string.IsNullOrEmpty(MODEL.lblDataVelocidadeResistente))
                    AddResultForcaVResist(ref f, ref numAvaliacao, Connection);

                //Agilidade
                if (!string.IsNullOrEmpty(MODEL.lblDataForcaAgilidade))
                    AddResultForcaAgilidade(ref f, ref numAvaliacao, Connection);

                //Força Explosiva Salto Horizontal
                if (!string.IsNullOrEmpty(MODEL.lblDataExplosivaH))
                    AddResultForcaExplosivaH(ref f, ref numAvaliacao, Connection);

                //Força Explosiva Salto Vertical
                if (!string.IsNullOrEmpty(MODEL.lblDataExplosivaV))
                    AddResultForcaExplosivaV(ref f, ref numAvaliacao, Connection);
*/
                Html += HtmlPdf.Replace("\n","<br/>");

            }
            return Html;
        }

        private static string AddResultDepressao(List<GT_RespAnsiedadeDepressao> Result, string SOCIO_NOME)
        {
            string sResult =Result.Select(x=>x.RESP_SUMMARY).FirstOrDefault().ToString();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (sResult.Length == 1) sResult = "01";

            if (!string.IsNullOrEmpty(sResult) && sResult.Length>=2)
            {
                sb.Append("<table style=\"margin-bottom:15px\"><tr style=\"background:blue;\"><td style=\"border:1px solid #333;height:33px;font-size:20px;font-align:center\">Ansiedade e Depressão</td></tr></table>");

                if (sResult.Substring(0, 1) == "1")
                {
                    //Se apresenta estado de ansiedade
                    sb.Append("Normalmente o estado de Ansiedade é uma experiência negativa mas os seus efeitos no dia-a-dia e até ");
                    sb.AppendFormat("na performance atlética pode ser positivo, negativo ou indiferente, consoante a sua personalidade.\nO/A {0} apresenta ", SOCIO_NOME);
                    sb.Append("valores que indicam estar sob um estado de ansiedade. É sugerido que se envolva num programa de actividade física, ");
                    sb.Append("dando enfâse aos exercícios aeróbicos.");
                }
                else
                {
                    //Se NÃO apresenta estado de ansiedade
                    sb.Append("Normalmente o estado de Ansiedade é uma experiência negativa mas os seus efeitos no dia-a-dia e até ");
                    sb.Append("na performance atlética pode ser positivo, negativo ou indiferente, consoante a sua personalidade.\n");
                    sb.Append("Os seu valores que indicam ausência de estado de ansiedade. Se pratica alguma actividade física, continue!");
                }

                sb.Append("\n\n");

                if (sResult.Substring(1, 1) == "1")
                {
                    //Se apresenta depressão
                    sb.Append("A depressão é caracterizada por um conjunto de sinais e sintomas que incluem a falta de humor, isolamento ");
                    sb.Append("social, perda de interesse nas actividades usuais e baixos níveis de energia.\n");
                    sb.AppendFormat("O/A {0} apresenta valores de um estado de depressão. Envolva-se em actividades desportivas de lazer. Estabeleça objectivos específicos para a sua vida.", SOCIO_NOME);
                }
                else
                {
                    //Se NÃO apresenta depressão
                    sb.Append("A depressão é caracterizada por um conjunto de sinais e sintomas que incluem a falta de humor, isolamento ");
                    sb.Append("social, perda de interesse nas actividades usuais e baixos níveis de energia.\n");
                    sb.AppendFormat("O/A {0} não apresenta valores que caracterizam um estado depressivo.", SOCIO_NOME);
                }

            }
            return sb.ToString();
        }

        private static string AddResultAutoConceito(List<GT_RespAutoConceito> Result, string SOCIO_NOME)
        {
            string sResult = Result.Select(x => x.RESP_SUMMARY).FirstOrDefault().ToString();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<table style=\"margin-top:30px;margin-bottom:15px\"><tr style=\"background:blue;\"><td style=\"border:1px solid #333;height:33px;font-size:20px;font-align:center\">Auto Conceito</td></tr></table>");

               if (Convert.ToInt32(sResult) <= Convert.ToInt32(67))
                {
                    //Se apresenta baixo auto conceito
                    sb.Append("Sumariamente o auto-conceito é o conceito que o indivíduo tem de si próprio.\n");
                    sb.AppendFormat("O/A {0} apresenta valores de baixo auto-conceito. Melhore a sua condição física ", SOCIO_NOME);
                    sb.Append("através de um plano de treino de força e cardiovascular, e intensidade moderada.");
                }
                else
                {
                    //Se NÃO apresenta baixo auto-conceito ou seja bom auto-conceito
                    sb.Append("Sumariamente o auto-conceito é o conceito que o indivíduo tem de si próprio.\n");
                    sb.AppendFormat("O/A {0} apresenta valores que indicam possuir um auto-conceito positivo.", SOCIO_NOME);
                }

            return sb.ToString();
        }

        private static string AddResultRiscoCoronario(List<GT_RespRisco> Result,int? IMC)
        {
            string sResult = string.Empty;
            string sCampos = string.Empty;
            string sTemp = string.Empty;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            var oDataReader = Result.FirstOrDefault();

            sTemp = Result.Select(x => x.RESP_SUMMARY).FirstOrDefault().ToString();

            if (sTemp == "0")
                sTemp = "baixo";
            else if (sTemp == "1")
                sTemp = "moderado";
            else if (sTemp == "2")
                sTemp = "elevado";

            sb.Append("<table style=\"margin-top:30px;margin-bottom:15px\"><tr style=\"background:blue;\"><td style=\"border:1px solid #333;height:33px;font-size:20px;font-align:center\">Risco Coronário</td></tr></table>");

            sb.AppendFormat("Tem um risco {0} para doenças cardiovasculares.\n\n", sTemp);

                //Historia Familiar
                if (oDataReader.radHeredMasc.Value==true || oDataReader.radHeredFem.Value==true)
                    sb.Append("Hereditariedade");
               sb.Append(oDataReader.radHeredMasc!=null && oDataReader.radHeredMasc.Value==true? "<li style=\"margin-bottom:0\">Familiar em 1º grau do sexo masculino falecido com doença cardiovascular antes dos 55 anos.</li>" : string.Empty);
               sb.Append(oDataReader.radHeredFem != null && oDataReader.radHeredFem.Value==true ? "<li> Familiar em 1º grau do sexo masculino falecido com doença cardiovascular antes dos 55 anos.</li>" : string.Empty);

                //Hábitos Tabágicos
                if (Convert.ToBoolean(oDataReader.radTabacFuma) || Convert.ToBoolean(oDataReader.radTabacFuma6))
                    sb.Append("Tabagismo\n");

           sb.Append(Convert.ToBoolean(oDataReader.radTabacFuma) ? "<li style=\"margin-bottom:0\">Fuma actualmente.</li>" : string.Empty);
            sb.Append(Convert.ToBoolean(oDataReader.radTabacFuma6) ? "<li>Deixou de fumar no últimos 6 meses.</li>" : string.Empty);

                //HiperColesterolemia
                if (Convert.ToBoolean(oDataReader.radColesterol1) || Convert.ToBoolean(oDataReader.radColesterol3) || Convert.ToBoolean(oDataReader.radColesterol5))
                    sb.Append("Hipercolesterolémia\n");

            sb.Append(Convert.ToBoolean(oDataReader.radColesterol1) ? "<li style=\"margin-bottom:0\">Tem colesterol superior a 200 mg/dl.</li>" : string.Empty);
            sb.Append(Convert.ToBoolean(oDataReader.radColesterol3) ? "<li style=\"margin-bottom:0\">Tem HDL (colesterol bom) inferior a 35 mg/dl.</li>" : string.Empty);
            sb.Append(Convert.ToBoolean(oDataReader.radColesterol5) ? "<li>Está medicado(a) para controlo do colesterol.</li>" : string.Empty);

                //Hipertensão
                if (Convert.ToBoolean(oDataReader.radTensao) || Convert.ToBoolean(oDataReader.radMedicacao))
                    sb.Append("Hipertensão\n");

            sb.AppendFormat(Convert.ToBoolean(oDataReader.radTensao) ? "<li style=\"margin-bottom:0\">Tensão arterial sistólica superior a 140 mm Hg ou tensão arterial distólica superior 90 mm Hg ({0} mm Hg e {1} mm Hg).</li>" : string.Empty,oDataReader.txtMaxSistolica+"-"+oDataReader.txtMinSistolica,oDataReader.txtMaxDistolica+"-"+oDataReader.txtMinDistolica);
            sb.Append(Convert.ToBoolean(oDataReader.radMedicacao) ? "<li>Está sob efeito de medicação para o controlo da tensão arterial.</li>" : string.Empty);

                //Glicose
                if (Convert.ToBoolean(oDataReader.radGlicose))
                    sb.Append("Intolerância à Glicose\n");

            sb.Append(Convert.ToBoolean(oDataReader.radGlicose)? "<li>Apresenta glicose sanguínea em jejum superior a 110 mg/dl.</li>" : string.Empty);

                //Inactividade Física				
                if (Convert.ToBoolean(oDataReader.radInactividade1)!=true)
                    sb.Append("Inactividade Física\n");

                //Obesidade
                    if (IMC > 30)
                        sb.Append("Obesidade\n");

                //Doença Cardiovascular
                if (!string.IsNullOrEmpty(oDataReader.txtCardiaca) || !string.IsNullOrEmpty(oDataReader.txtVascular) || !string.IsNullOrEmpty(oDataReader.txtCerebroVascular) || !string.IsNullOrEmpty(oDataReader.txtCardioVascularOutras))
                    sb.Append("Doença Cardiovascular\n");

                //Doença Pulmonar
                if (!string.IsNullOrEmpty(oDataReader.txtObstrucao) || !string.IsNullOrEmpty(oDataReader.txtAsma) || !string.IsNullOrEmpty(oDataReader.txtFibrose) || !string.IsNullOrEmpty(oDataReader.txtPulmomarOutras))
                    sb.Append("Doença Pulmonar\n");

                //Doença Metabólica
                if (!string.IsNullOrEmpty(oDataReader.txtDiabetes1) || !string.IsNullOrEmpty(oDataReader.txtDiabetes2) || !string.IsNullOrEmpty(oDataReader.txtRenais)
                    || !string.IsNullOrEmpty(oDataReader.txtFigado) || !string.IsNullOrEmpty(oDataReader.txtMetabolicaOutras) || !string.IsNullOrEmpty(oDataReader.txtTiroide))
                    sb.Append("Doença Metabólica\n");

                //Sinais e sintomas sugestivos de doença cardiovascular ou pulmunar
                if (Convert.ToBoolean(oDataReader.chkDor.Value) || Convert.ToBoolean(oDataReader.chkRespiracao.Value) || Convert.ToBoolean(oDataReader.chkTonturas)
                    || Convert.ToBoolean(oDataReader.chkDispeneia) || Convert.ToBoolean(oDataReader.chkEdema) || Convert.ToBoolean(oDataReader.chkPalpitacoes)
                    || Convert.ToBoolean(oDataReader.chkClaudicacao) || Convert.ToBoolean(oDataReader.chkMurmurio) || Convert.ToBoolean(oDataReader.chkfadiga))
                    sb.Append("Apresenta sinais e sintomas sugestivos de doença cardiovascular ou pulmunar\n");
            return sb.ToString();
        }

        private static string AddResultFuncional(List<GT_RespFuncional> Result)
        {
            string sResult = string.Empty;
            string sCampos = string.Empty;
            string sTemp = string.Empty;
            System.Text.StringBuilder sbTemp = new System.Text.StringBuilder();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<table style=\"margin-top:30px;margin-bottom:15px\"><tr style=\"background:blue;\"><td style=\"border:1px solid #333;height:33px;font-size:20px;font-align:center\">Avaliação Funcional</td></tr></table>");

            sTemp = Result.Select(x => x.RESP_SUMMARY).FirstOrDefault().ToString();
                
                sb.AppendFormat("A sua pontuação foi de {0} pontos em 21 possíveis. Obteve:", sTemp);

                sbTemp = GetTestes(Result, 0);
                if (sbTemp.Length > 0)
                {
                    sb.Append("\n\n\t\tPontuação 0 no(s) seguinte(s) teste(s):");
                    sb.Append(sbTemp.ToString() + "\n");
                    sb.Append("É aconselhado uma avaliação médico-desportiva da zona de dor considerando o(s) ");
                    sb.Append("modelo(s) de movimento que provocou a dor.");
                }

                sbTemp = GetTestes(Result, 1);
                if (sbTemp.Length > 0)
                {
                    sb.Append("\n\n\t\tPontuação 1 no(s) seguinte(s) teste(s):");
                    sb.Append(sbTemp.ToString() + "\n\n");
                    sb.Append("Demonstra que não possui uma base de estabilidade e mobilidade, e por isso muito ");
                    sb.Append("provavelmente poderá vivenciar microtraumas, fraca eficiência e fraca técnica na ");
                    sb.Append("realização de algumas técnicas/movimentos desportivos. A flexibilidade e a força ");
                    sb.Append("deverão ser melhor avaliadas nas áreas em questão.");
                }

                sbTemp = GetTestes(Result, 2);
                if (sbTemp.Length > 0)
                {
                    sb.Append("\n\n\t\tPontuação 2 no(s) seguinte(s) teste(s):");
                    sb.Append(sbTemp.ToString() + "\n\n");
                    sb.Append("Estas zonas deverão ser consideradas prioritárias no fortalecimento e na flexibilidade. ");
                    sb.Append("É aconselhado o desenvolvimento de exercícios complementares, e um treino desportivo específico ");
                    sb.Append("que vizem as esta(s) área(s) de limitação.");
                }

                sbTemp = GetTestes(Result, 3);
                if (sbTemp.Length > 0)
                {
                    sb.Append("\n\n\t\tPontuação 3 no(s) seguinte(s) teste(s):");
                    sb.Append(sbTemp.ToString() + "\n\n");
                    sb.Append("Aqui apresenta uma mobilidade e estabilidade óptima, apropriada para um modelo particular de ");
                    sb.Append("movimento, contudo, a avaliação deverá ser realizada periodicamente a fim de detectar pequenos ");
                    sb.Append("desequilíbrios passíveis de serem provocados pelo treino.");
                }

            return sb.ToString();
        }

        private static string AddResultSaude(List<GT_RespProblemasSaude> Result)
        {
            string sResult = string.Empty;
            string sCampos = string.Empty;
            string sTemp = string.Empty;
            System.Text.StringBuilder sbTemp = new System.Text.StringBuilder();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append("<table style=\"margin-top:30px;margin-bottom:15px\"><tr style=\"background:blue;\"><td style=\"border:1px solid #333;height:33px;font-size:20px;font-align:center\">Problemas de Saúde</td></tr></table>");

            sTemp = Result.Select(x => x.txtInactividade).FirstOrDefault();
                if (!string.IsNullOrEmpty(sTemp))
                {
                    sb.AppendFormat("Foi referido o/a {0} como factor limitativo à realização de actividade física.", sTemp);
                }
                else
                {
                    sb.Append("Não existe quaquer razão médica que limite a sua actividade física.");
                }
            
            return sb.ToString();
        }







        private static System.Text.StringBuilder GetTestes(List<GT_RespFuncional> Result, int sPontuacao)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            var oDataReader = Result.FirstOrDefault();

            if (oDataReader.RESP_01 == sPontuacao)
            { sb.AppendFormat("\n"); sb.AppendFormat("\t\t\t\tDeep Squat"); }

            if (oDataReader.RESP_02 == sPontuacao)
                sb.AppendFormat("\n\t\t\t\tHurdle Step");

            if (oDataReader.RESP_03 == sPontuacao)
                sb.AppendFormat("\n\t\t\t\tIn-Line Lunge");

            if (oDataReader.RESP_04 == sPontuacao)
                sb.AppendFormat("\n\t\t\t\tShoulder Mobility");

            if (oDataReader.RESP_05 == sPontuacao)
                sb.AppendFormat("\n\t\t\t\tActive straight-Leg raise");

            if (oDataReader.RESP_06 == sPontuacao)
                sb.AppendFormat("\n\t\t\t\tTrunk Stability Push Up");

            if (oDataReader.RESP_07 == sPontuacao)
                sb.AppendFormat("\n\t\t\t\tRotary Stability");

            return sb;
        }

    }
}