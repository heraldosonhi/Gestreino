using Gestreino.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Ajax.Utilities;
using iText.StyledXmlParser.Jsoup.Nodes;
using DocumentFormat.OpenXml.EMMA;
//using System.Web.Http.Results;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Windows;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using System.Web.Services.Description;
using iText.Kernel.Pdf.Layer;
//using Org.BouncyCastle.Ocsp;
using System.Web.UI;
using DocumentFormat.OpenXml.Bibliography;
using static ClosedXML.Excel.XLPredefinedFormat;
using System.Data.Entity;
using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel;
using System.Collections;
using Gestreino.Classes;
using Gestreino;

namespace Gestreino.Classes
{
    public class PDFReports
    {
        public static string BodyMassReport(int? Id,/*List<SP_GT_ENT_TREINO_Result> treino*/ string path, string logo)
        {

            string Html = null;

            using (GESTREINO_Entities databaseManager = new GESTREINO_Entities())
            {

                var treino = databaseManager.GT_Treino.Where(x => x.ID == Id).ToList();
                var tipotreinoID = treino.First().GT_TipoTreino_ID;
                var treinosp = databaseManager.SP_GT_ENT_TREINO(Id, null, tipotreinoID, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "R").ToList();

                var exercicio = databaseManager.GT_ExercicioTreino.Where(x=>x.GT_Treino_ID == Id).ToList();
                var exIds = exercicio.Select(x=>x.GT_Exercicio_ID).ToList();
                var exSeries = exercicio.Select(x => x.GT_Series_ID).ToList();
                var exRepeticoes = exercicio.Select(x => x.GT_Repeticoes_ID).ToList();
                var exDescanso = exercicio.Select(x => x.GT_TempoDescanso_ID).ToList();

                var images = (from j1 in databaseManager.GT_Exercicio_ARQUIVOS
                                               join j2 in databaseManager.GRL_ARQUIVOS on j1.ARQUIVOS_ID equals j2.ID
                                               where exIds.Contains(j1.GT_Exercicio_ID)
                                               select new { j1.ID, j1.GT_Exercicio_ID,j2.CAMINHO_URL }).ToList();


                var ename = databaseManager.GT_Exercicio.Where(x => exIds.Contains(x.ID)).ToList();
                var eseries = databaseManager.GT_Series.Where(x => exSeries.Contains(x.ID)).ToList();
                var erepeticoes = databaseManager.GT_Repeticoes.Where(x => exRepeticoes.Contains(x.ID)).ToList();
                var edescanso = databaseManager.GT_TempoDescanso.Where(x => exDescanso.Contains(x.ID)).ToList();

                var tnome = treinosp.First().NOME_PROPIO + " " + treinosp.First().APELIDO;

                // INSTITUICAO
                var INST = (from j1 in databaseManager.INST_APLICACAO join j2 in databaseManager.INST_APLICACAO_ENDERECOS on j1.ID equals j2.INST_APLICACAO_ID join j3 in databaseManager.INST_APLICACAO_CONTACTOS on j1.ID equals j3.INST_APLICACAO_ID where j1.ID == Classes.Configs.INST_INSTITUICAO_ID select new { j2.MORADA, j3.TELEFONE, j3.TELEFONE_ALTERNATIVO, j3.EMAIL }).ToList();
                Html += "<html>";
                Html += "<link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css' rel='stylesheet' integrity='sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH' crossorigin='anonymous'>";
                Html += "<style> .footertext{font-size:9px;font-weight:900;} .p4{padding:.4rem;margin-bottom:0;margin-top:0} .tabletext{display: block;transform: rotate(90deg);padding:3rem;position:relative;width:inherit;bottom:.1rem}   .r-table td,.r-table th{padding:4px 5px!important;text-align:center}.r-table th span{padding: 0rem;}.r-table td,.r-table th,.text-center{text-align:center}@page{margin:1.2cm 1.2cm .4cm 1cm}body{font-family:Arial,Helvetica,sans-serif;font-size:14px;}.border{border:1px solid #000}.bg-ddd,.r-table td,.r-table th{border:1px solid #333}ul{padding:0;list-style:none}li{margin-bottom:10px}.d-flex{display:flex}.j-content-space-between{justify-content:space-between}.bg-ddd{background:#ddd}.text-md{font-size:12px}.main-header .header-img{text-align:center;padding-bottom:20px;border-bottom:2px solid #000}.main-content .section{margin-bottom:5px}.main-content .section-header{padding:2px 4px;font-weight:bolder;background-color:#e4e4e4;border:1px solid #000;margin-bottom:8px}.document-title{margin:10px auto}.section-content{padding:auto 15px}.main-footer .candidato p:first-child{width:7%}.main-footer .candidato{margin-bottom:0}.main-footer .candidato-obs{margin-top:0}.main-footer .candidato p:last-child{width:93%;padding:2px;border-bottom:1px solid #000}strong{font-weight:bolder}.item-tb{padding-bottom:5px}.item-l,.item-r{width:50%}.item-doc{width:33.333%}.p-y-1{padding:2px auto}table{width:100%}.main-header-title .entity{padding-bottom:2px}.main-header-title .country{letter-spacing:10px}.main-footer{width:100%;height:200px;position:absolute;z-index:100;bottom:0}.main-content{padding-bottom:190px}.r-table{width:733px}.r-table th{background-color:#ddd;height:143px}.r-table td:nth-of-type(3),.r-table th:nth-of-type(3){border:0!important;background-color:#fff}.text-left{text-align:left!important}.mt-1{margin-top:.2rem}</style>";
                Html += "<body>";
               
                Html += "<div class=bg-ddd><div class=row><div class=col-md-12>";
                Html += "<h1 class=\"p4\" style=\"font-weight:900;\">Ginásio Gestreino</h1></div></div><table><tr><td style=width:250px><h3 class=\"p4\" style=\"font-weight:900;\">Número: <small>"+treinosp.First().NUMERO+"</small></h3><td><h3 style=\"font-weight:900;\">Nome: <small>"+ tnome + "</small></h3></table></div><div class=\"bg-ddd mt-1\"><table><tr><td style=width:250px><h3 class=\"p4\" style=\"font-weight:900;margin-bottom:0;margin-top:0\">Data de Início: <small>"+treinosp.First().DATA_INICIO+"</small></h3><td><h3 style=\"font-weight:900;margin-bottom:0;margin-top:0\">Factores de risco: <small>AC AF RR</small></h3><tr><td style=width:250px><h3 class=\"p4\" style=\"font-weight:900;margin-bottom:0;margin-top:0\">Duração do plano: <small>5</small></h3><td><h3 style=\"font-weight:900;margin-bottom:0;margin-top:0\">Objectivos: <small>PP BM BE</small></h3><tr><td style=width:250px><h3 class=\"p4\" style=\"font-weight:900;\">Protocolo: <small>6</small></h3></table></div>";
                Html += "<div class=\"bg-ddd mt-1\"><div class=row><div class=col-md-12><center><h2 style=\"font-weight:900;margin-bottom:0\" >TREINO MUSCULAÇÃO</h1></center></div></div></div>";
                Html += "<div class=\"mt-1\"  style=\"height:420px\"><div class=row><div class=col-md-12><table class=r-table><thead><tr><th style=width:33px><span class=tabletext>Altura&nbsp;Banco</span><th style=width:33px><span class=tabletext>Inclinação</span><th style=width:33px><th style=width:123px colspan=3>Exercício<th style=width:33px><span class=tabletext>Alongamento</span><th style=width:83px>Séries<th style=width:100px>Descanso<th style=width:100px>Carga<th style=width:33px><span class=tabletext>Repetições</span><th style=width:33px><span class=tabletext>Ajust.&nbsp;Carga</span>";
                Html += "<tbody>";

                int c = 0;
                foreach (var ex in exercicio)
                {
                    c++;
                    var enome = ename.Where(x => x.ID == ex.GT_Exercicio_ID).Select(x => x.NOME).FirstOrDefault();
                    var eserie = eseries.Where(x => x.ID == ex.GT_Series_ID).Select(x => x.SERIES).FirstOrDefault();
                    var erepeticao = erepeticoes.Where(x => x.ID == ex.GT_Repeticoes_ID).Select(x => x.REPETICOES).FirstOrDefault();
                    var edescansos= edescanso.Where(x => x.ID == ex.GT_TempoDescanso_ID).Select(x => x.TEMPO_DESCANSO).FirstOrDefault();

                    Html += "<tr>";
                    Html += "<td class=bg-ddd></td>";
                    Html += "<td class=bg-ddd></td>";
                    Html += "<td>X</td>";
                    Html += "<td>"+c+".</td>";
                    Html += "<td class=bg-ddd>"+(ex.CARGA_USADA??0).ToString("G29")+"</td>";
                    Html += "<td class=text-left>"+enome+"</td>";
                    Html += "<td class=bg-ddd>0</td>";
                    Html += "<td>"+eserie+"</td>";
                    Html += "<td>"+ edescansos + "</td>";
                    Html += "<td>X</td>";
                    Html += "<td>"+ erepeticao + "</td>";
                    Html += "<td></td>";
                    Html += "</tr>";
                }

                Html += "</tbody></table></div></div></div>";
                Html += "<h1>&nbsp;</h1>";
                Html += "<table style=width:758px><tr>";

                foreach (var ex in exercicio)
                {

                    var img = images.Where(x => x.GT_Exercicio_ID == ex.GT_Exercicio_ID).OrderByDescending(x=>x.ID).Select(x => x.CAMINHO_URL).FirstOrDefault();


                    Html += "<td>"+img+ "<span style=margin-left:55px>1</span><br><img src=\"/Assets/images/user-avatar.jpg\" style=\"border:1px solid #333;width:100px\"></td>";

               //     Html += "<td><span style=margin-left:55px>1</span><br><img src=\"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSFhU3gyDQM2_Zr3kU9hahczQwgNU0Vvu4uvA&s\"style=\"border:1px solid #333;width:100px\"></td>";
                }

                Html += "</tr></table>";
                Html+= "<div class=\"bg-ddd mt-1\"><h5 class=\"p4\" style=\"font-weight:900;margin-bottom:0;font-size:13px;\"> Regras da sala de exercício:</h5><table class=\"table footertext\" style=\"margin-bottom:0\"><tr><td class=col-md-6>1. Faça sempre o aquecimento com exercícios de alongamentos<br>2. Utilize toalha para evitar o contacto directo com os equipamentos<br>3. Utilize ténis apropriados para a prática da actividade<br/>Obrigado pela sua compreensão<td class=col-md-6>4. Beba água antes, durante e após cada sessão<br>5. Não é permitido levar telemóveis e outros objectos para a sala de exercício<br>6. Cumpra com a prescrição dos instrutores e esclareça todas as dúvidas</table></div>";
                
                /*Html += "   <div style=\"border-top: 2px solid black; padding-top: 2px; margin-top: 12px\">";
                Html += "   </div>";
                Html += "   <p class=\"text-center\" style=\"margin: 0; color: red\">";
                Html += "       <small>";
                Html += "           " + Configs.INST_INSTITUICAO_NOME + " - " + Configs.INST_INSTITUICAO_ENDERECO + " <br>";
                Html += "           Tel. " + INST[0].TELEFONE + " / " + INST[0].TELEFONE_ALTERNATIVO + " Email: " + INST[0].EMAIL + "";
                Html += "       </small>";
                Html += "   </p>";
                Html += " </footer>";*/
                Html += " </body>";
                Html += " </html>";
            }
            return Html;
        }
    }
}