using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
//using iText.Kernel.Pdf.Canvas.Wmf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using Gestreino.Classes;

namespace Gestreino.Classes
{
    public class XLSReports
    {
        private GESTREINO_Entities databaseManager = new GESTREINO_Entities();

        /*
       *******************************************
       *******************************************
       DATATABLES
       *******************************************
       *******************************************
       */
        
        public byte[] ExportToExcel(string section, IList query)
        {
            switch (section)
            {
                case "athletes":
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var worksheet = wb.Worksheets.Add("Sample Sheet");
                        var currentRow = 1;
                        worksheet.Cell(currentRow, 1).Value = "ATLETA";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Value = "N° SÓCIO";
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 3).Value = "SEXO";
                        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 4).Value = "DATA DE NASCIMENTO";
                        worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 5).Value = "ESTADO CIVIL";
                        worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 6).Value = "NATURALIDADE";
                        worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 7).Value = "NACIONALIDADE";
                        worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 8).Value = "TELEFONE";
                        worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 9).Value = "TELEFONE ALTERNATIVO";
                        worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 10).Value = "EMAIL";
                        worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 11).Value = "NIF";
                        worksheet.Cell(currentRow, 11).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 12).Value = "INSERÇÃO";
                        worksheet.Cell(currentRow, 12).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 13).Value = "DATA INSERÇÃO";
                        worksheet.Cell(currentRow, 13).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 14).Value = "ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 14).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 15).Value = "DATA ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 15).Style.Font.Bold = true;
                        worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        for (int i = 0; i < query.Count; i++)
                        {
                            object NOME = query[i].GetType().GetProperty("NOME").GetValue(query[i], null);
                            object NUMERO = query[i].GetType().GetProperty("PES_NUMERO").GetValue(query[i], null);
                            object SEXO = query[i].GetType().GetProperty("SEXO").GetValue(query[i], null);
                            object DATA = query[i].GetType().GetProperty("DATA_NASCIMENTO").GetValue(query[i], null);
                            object CIVIL = query[i].GetType().GetProperty("ESTADO_CIVIL").GetValue(query[i], null);
                            object NATURALIDADE = query[i].GetType().GetProperty("NATURALIDADE_PAIS").GetValue(query[i], null)+" - "+ query[i].GetType().GetProperty("NATURALIDADE_CIDADE").GetValue(query[i], null)+" - "+ query[i].GetType().GetProperty("NATURALIDADE_MUN").GetValue(query[i], null);
                            object NACIONALIDADE = query[i].GetType().GetProperty("NACIONALIDADE").GetValue(query[i], null);
                            object TELEFONE = query[i].GetType().GetProperty("TELEFONE").GetValue(query[i], null);
                            object TELEFONEALTERNATIVO = query[i].GetType().GetProperty("TELEFONE_ALTERNATIVO").GetValue(query[i], null);
                            object EMAIL = query[i].GetType().GetProperty("EMAIL").GetValue(query[i], null);
                            object NIF = query[i].GetType().GetProperty("NIF").GetValue(query[i], null);
                            object INSERCAO = query[i].GetType().GetProperty("INSERCAO").GetValue(query[i], null);
                            object ACTUALIZACAO = query[i].GetType().GetProperty("ACTUALIZACAO").GetValue(query[i], null);
                            object DATAINSERCAO = query[i].GetType().GetProperty("DATA_INSERCAO").GetValue(query[i], null);
                            object DATAACTUALIZACAO = query[i].GetType().GetProperty("DATA_ACTUALIZACAO").GetValue(query[i], null);
                       
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = NOME != null ? NOME.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 2).Value = NUMERO != null ? NUMERO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 3).Value = SEXO != null ? SEXO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 4).Value = DATA != null ? DATA.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 5).Value = CIVIL != null ? CIVIL.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 6).Value = NATURALIDADE != null ? NATURALIDADE.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 7).Value = NACIONALIDADE != null ? NACIONALIDADE.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 8).Value = TELEFONE != null ? TELEFONE.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 9).Value = TELEFONEALTERNATIVO != null ? TELEFONEALTERNATIVO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 10).Value = EMAIL != null ? EMAIL.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 11).Value = NIF!=null?NIF.ToString():string.Empty;
                            worksheet.Cell(currentRow, 12).Value = INSERCAO != null ? INSERCAO.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 13).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 14).Value = ACTUALIZACAO != null ? ACTUALIZACAO.ToString() : string.Empty;  
                            worksheet.Cell(currentRow, 15).Value = DATAACTUALIZACAO != null ? DATAACTUALIZACAO.ToString() : string.Empty; 
                        }

                        worksheet.Columns().AdjustToContents();
                        using (MemoryStream myMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(myMemoryStream);

                            // return memory stream as byte array
                            return myMemoryStream.ToArray();
                        }
                    }
                case "GetUsersIdentification":
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var worksheet = wb.Worksheets.Add("Sample Sheet");
                        var currentRow = 1;

                        worksheet.Cell(currentRow, 1).Value = "ATLETA";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Value = "IDENTIFICAÇÃO";
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 3).Value = "NÚMERO";
                        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 4).Value = "DATA EMISSÃO";
                        worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 5).Value = "DATA VALIDADE";
                        worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 6).Value = "LOCAL DE EMISSÃO";
                        worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 7).Value = "ÓRGÃO EMISSOR";
                        worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 8).Value = "OBSERVAÇÕES";
                        worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 9).Value = "INSERÇÃO";
                        worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 10).Value = "DATA INSERÇÃO";
                        worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 11).Value = "ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 11).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 12).Value = "DATA ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 12).Style.Font.Bold = true;
                        worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        for (int i = 0; i < query.Count; i++)
                        {
                            object NOME = query[i].GetType().GetProperty("PES_NOME").GetValue(query[i], null);
                            object IDENTIFICACAO = query[i].GetType().GetProperty("IDENTIFICACAO").GetValue(query[i], null);
                            object NUMERO = query[i].GetType().GetProperty("NUMERO").GetValue(query[i], null);
                            object DATAEMISSAO = query[i].GetType().GetProperty("DATA_EMISSAO").GetValue(query[i], null);
                            object DATAVALIDADE = query[i].GetType().GetProperty("DATA_VALIDADE").GetValue(query[i], null);
                            object LOCALEMISSAO = query[i].GetType().GetProperty("CIDADE").GetValue(query[i], null);
                            object ORGAOEMISSOR = query[i].GetType().GetProperty("ORGAO_EMISSOR").GetValue(query[i], null);
                            object OBSERVACOES = query[i].GetType().GetProperty("OBSERVACOES").GetValue(query[i], null);
                            object INSERCAO = query[i].GetType().GetProperty("INSERCAO").GetValue(query[i], null);
                            object DATAINSERCAO = query[i].GetType().GetProperty("DATA_INSERCAO").GetValue(query[i], null);
                            object ACTUALIZACAO = query[i].GetType().GetProperty("ACTUALIZACAO").GetValue(query[i], null);
                            object DATAACTUALIZACAO = query[i].GetType().GetProperty("DATA_ACTUALIZACAO").GetValue(query[i], null);

                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = NOME != null ? NOME.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 2).Value = IDENTIFICACAO != null ? IDENTIFICACAO.ToString() : string.Empty;  ;
                            worksheet.Cell(currentRow, 3).Value = NUMERO != null ? NUMERO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 4).Value = DATAEMISSAO != null ? DATAEMISSAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 5).Value = DATAVALIDADE != null ? DATAVALIDADE.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 6).Value = LOCALEMISSAO != null ? LOCALEMISSAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 7).Value = ORGAOEMISSOR != null ? ORGAOEMISSOR.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 8).Value = OBSERVACOES != null ? OBSERVACOES.ToString() : string.Empty;  ;
                            worksheet.Cell(currentRow, 9).Value = INSERCAO != null ? INSERCAO.ToString() : string.Empty;  ;
                            worksheet.Cell(currentRow, 10).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 11).Value = ACTUALIZACAO != null ? ACTUALIZACAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 12).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty;
                        }

                        worksheet.Columns().AdjustToContents();
                        using (MemoryStream myMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(myMemoryStream);

                            // return memory stream as byte array
                            return myMemoryStream.ToArray();
                        }
                    }
                case "GetUsersProfessional":
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var worksheet = wb.Worksheets.Add("Sample Sheet");
                        var currentRow = 1;

                        //worksheet.Cell(currentRow, 1).Value = "ATLETA";
                        //worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 1).Value = "EMPRESA";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Value = "PROFISSÃO";
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 3).Value = "CONTRACTO";
                        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 4).Value = "REGIME";
                        worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 5).Value = "DATA INICIAL";
                        worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 6).Value = "DATA FINAL";
                        worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 7).Value = "DESCRIÇÃO";
                        worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 8).Value = "INSERÇÃO";
                        worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 9).Value = "DATA INSERÇÃO";
                        worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 10).Value = "ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 11).Value = "DATA ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 11).Style.Font.Bold = true;
                        worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        for (int i = 0; i < query.Count; i++)
                        {
                            //object NOME = query[i].GetType().GetProperty("PES_NOME").GetValue(query[i], null);
                            object EMPRESA = query[i].GetType().GetProperty("EMPRESA").GetValue(query[i], null);
                            object FUNCAO = query[i].GetType().GetProperty("PROFISSAO").GetValue(query[i], null);
                            object CONTRACTO = query[i].GetType().GetProperty("CONT_NOME").GetValue(query[i], null);
                            object REGIME = query[i].GetType().GetProperty("REGIME_NOME").GetValue(query[i], null);
                            object DATAINICIAL = query[i].GetType().GetProperty("DATA_INICIO").GetValue(query[i], null);
                            object DATAFIM = query[i].GetType().GetProperty("DATA_FIM").GetValue(query[i], null);
                            object DESCRICAO = query[i].GetType().GetProperty("DESCRICAO").GetValue(query[i], null);
                            object INSERCAO = query[i].GetType().GetProperty("INSERCAO").GetValue(query[i], null);
                            object DATAINSERCAO = query[i].GetType().GetProperty("DATA_INSERCAO").GetValue(query[i], null);
                            object ACTUALIZACAO = query[i].GetType().GetProperty("ACTUALIZACAO").GetValue(query[i], null);
                            object DATAACTUALIZACAO = query[i].GetType().GetProperty("DATA_ACTUALIZACAO").GetValue(query[i], null);

                            currentRow++;
                            //worksheet.Cell(currentRow, 1).Value = NOME;
                            worksheet.Cell(currentRow, 1).Value = EMPRESA != null ? EMPRESA.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 2).Value = FUNCAO != null ? FUNCAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 3).Value = CONTRACTO != null ? CONTRACTO.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 4).Value = REGIME != null ? REGIME.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 5).Value = DATAINICIAL != null ? DATAINICIAL.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 6).Value = DATAFIM != null ? DATAFIM.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 7).Value = DESCRICAO != null ? DESCRICAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 8).Value = INSERCAO != null ? INSERCAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 9).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 10).Value = ACTUALIZACAO != null ? ACTUALIZACAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 11).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty;
                        }

                        worksheet.Columns().AdjustToContents();
                        using (MemoryStream myMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(myMemoryStream);

                            // return memory stream as byte array
                            return myMemoryStream.ToArray();
                        }
                    }
                case "GetUsersFamily":
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var worksheet = wb.Worksheets.Add("Sample Sheet");
                        var currentRow = 1;

                        //worksheet.Cell(currentRow, 1).Value = "ATLETA";
                        //worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 1).Value = "AGREGADO";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Value = "NOME";
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 3).Value = "PROFISSÃO ";
                        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 4).Value = "TELEFONE";
                        worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 5).Value = "TELEFONE ALTERNATIVO";
                        worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 6).Value = "FAX";
                        worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 7).Value = "EMAIL";
                        worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 8).Value = "URL";
                        worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 9).Value = "ENDERECO";
                        worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 10).Value = "MORADA";
                        worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 11).Value = "RUA";
                        worksheet.Cell(currentRow, 11).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 12).Value = "NÚMERO";
                        worksheet.Cell(currentRow, 12).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 13).Value = "INSERÇÃO";
                        worksheet.Cell(currentRow, 13).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 14).Value = "DATA INSERÇÃO";
                        worksheet.Cell(currentRow, 14).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 15).Value = "ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 15).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 16).Value = "DATA ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 16).Style.Font.Bold = true;
                        worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        for (int i = 0; i < query.Count; i++)
                        {
                            //object UTILIZADOR = query[i].GetType().GetProperty("PES_NOME").GetValue(query[i], null);
                            object AGREGADO = query[i].GetType().GetProperty("AGREGADO").GetValue(query[i], null);
                            object NOME = query[i].GetType().GetProperty("NOME").GetValue(query[i], null);
                            object PROFISSAO = query[i].GetType().GetProperty("PROFISSAO").GetValue(query[i], null);
                            object TELEFONE = query[i].GetType().GetProperty("TELEFONE").GetValue(query[i], null);
                            object TELEFONEALTERNATIVO = query[i].GetType().GetProperty("TELEFONE_ALTERNATIVO").GetValue(query[i], null);
                            object FAX = query[i].GetType().GetProperty("FAX").GetValue(query[i], null);
                            object EMAIL = query[i].GetType().GetProperty("EMAIL").GetValue(query[i], null);
                            object URL = query[i].GetType().GetProperty("URL").GetValue(query[i], null);
                            object ENDERECO = query[i].GetType().GetProperty("CIDADE_NOME").GetValue(query[i], null);
                            object MORADA = query[i].GetType().GetProperty("MORADA").GetValue(query[i], null);
                            object RUA = query[i].GetType().GetProperty("RUA").GetValue(query[i], null);
                            object NUMERO = query[i].GetType().GetProperty("NUMERO").GetValue(query[i], null);
                            object INSERCAO = query[i].GetType().GetProperty("INSERCAO").GetValue(query[i], null);
                            object DATAINSERCAO = query[i].GetType().GetProperty("DATA_INSERCAO").GetValue(query[i], null);
                            object ACTUALIZACAO = query[i].GetType().GetProperty("ACTUALIZACAO").GetValue(query[i], null);
                            object DATAACTUALIZACAO = query[i].GetType().GetProperty("DATA_ACTUALIZACAO").GetValue(query[i], null);

                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = AGREGADO != null ? AGREGADO.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 2).Value = NOME != null ? NOME.ToString() : string.Empty;  
                            worksheet.Cell(currentRow, 3).Value = PROFISSAO != null ? PROFISSAO.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 4).Value = TELEFONE != null ? TELEFONE.ToString() : string.Empty;  
                            worksheet.Cell(currentRow, 5).Value = TELEFONEALTERNATIVO != null ? TELEFONEALTERNATIVO.ToString() : string.Empty;  
                            worksheet.Cell(currentRow, 6).Value = FAX != null ? FAX.ToString() : string.Empty;  
                            worksheet.Cell(currentRow, 7).Value = EMAIL != null ? EMAIL.ToString() : string.Empty;  
                            worksheet.Cell(currentRow, 8).Value = URL != null ? URL.ToString() : string.Empty;  
                            worksheet.Cell(currentRow, 9).Value = ENDERECO != null ? ENDERECO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 10).Value = MORADA != null ? MORADA.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 11).Value = RUA != null ? RUA.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 12).Value = NUMERO != null ? NUMERO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 13).Value = INSERCAO != null ? INSERCAO.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 14).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 15).Value = ACTUALIZACAO != null ? ACTUALIZACAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 16).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty;
                        }

                        worksheet.Columns().AdjustToContents();
                        using (MemoryStream myMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(myMemoryStream);

                            // return memory stream as byte array
                            return myMemoryStream.ToArray();
                        }
                    }
                case "GetUsersDisability":
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var worksheet = wb.Worksheets.Add("Sample Sheet");
                        var currentRow = 1;

                        //worksheet.Cell(currentRow, 1).Value = "ATLETA";
                        //worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 1).Value = "DEFICIÊNCIA";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Value = "GRAU";
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 3).Value = "DESCRIÇÃO ";
                        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 4).Value = "INSERÇÃO";
                        worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 5).Value = "DATA INSERÇÃO";
                        worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 6).Value = "ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 7).Value = "DATA ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                        worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        for (int i = 0; i < query.Count; i++)
                        {
                            //object UTILIZADOR = query[i].GetType().GetProperty("PES_NOME").GetValue(query[i], null);
                            object DEFICIENCIA = query[i].GetType().GetProperty("NOME").GetValue(query[i], null);
                            object GRAU = query[i].GetType().GetProperty("GRAU").GetValue(query[i], null);
                            object DESCRICAO = query[i].GetType().GetProperty("DESCRICAO").GetValue(query[i], null);
                            object INSERCAO = query[i].GetType().GetProperty("INSERCAO").GetValue(query[i], null);
                            object DATAINSERCAO = query[i].GetType().GetProperty("DATA_INSERCAO").GetValue(query[i], null);
                            object ACTUALIZACAO = query[i].GetType().GetProperty("ACTUALIZACAO").GetValue(query[i], null);
                            object DATAACTUALIZACAO = query[i].GetType().GetProperty("DATA_ACTUALIZACAO").GetValue(query[i], null);

                            currentRow++;
                            //worksheet.Cell(currentRow, 1).Value = UTILIZADOR;
                            worksheet.Cell(currentRow, 1).Value = DEFICIENCIA != null ? DEFICIENCIA.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 2).Value = GRAU != null ? GRAU.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 3).Value = DESCRICAO != null ? DESCRICAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 4).Value = INSERCAO != null ? INSERCAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 5).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 6).Value = ACTUALIZACAO != null ? ACTUALIZACAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 7).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty; 
                        }

                        worksheet.Columns().AdjustToContents();
                        using (MemoryStream myMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(myMemoryStream);

                            // return memory stream as byte array
                            return myMemoryStream.ToArray();
                        }
                    }
                case "gttreino":
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var worksheet = wb.Worksheets.Add("Sample Sheet");
                        var currentRow = 1;

                        worksheet.Cell(currentRow, 1).Value = "ATLETA";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Value = "N° SÓCIO";
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 3).Value = "TREINO";
                        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 4).Value = "PERIODIZAÇÃO";
                        worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 5).Value = "DATA INÍCIO";
                        worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 6).Value = "DATA FIM ";
                        worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 7).Value = "OBSERVAÇÕES";
                        worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 8).Value = "INSERÇÃO";
                        worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 9).Value = "DATA INSERÇÃO";
                        worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 10).Value = "ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 11).Value = "DATA ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 11).Style.Font.Bold = true;
                        worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        for (int i = 0; i < query.Count; i++)
                        {
                            object ATLETA = query[i].GetType().GetProperty("NOME_PROPIO").GetValue(query[i], null) +" "+ query[i].GetType().GetProperty("APELIDO").GetValue(query[i], null);
                            object NUMERO = query[i].GetType().GetProperty("NUMERO").GetValue(query[i], null);
                            object TREINO = query[i].GetType().GetProperty("GT_TipoTreino_NOME").GetValue(query[i], null);
                            object PERIODO = query[i].GetType().GetProperty("PERIODIZACAO").GetValue(query[i], null);
                            object DATAINI = query[i].GetType().GetProperty("DATA_INICIO").GetValue(query[i], null);
                            object DATAFIM = query[i].GetType().GetProperty("DATA_FIM").GetValue(query[i], null);
                            object OBSERVACAO = query[i].GetType().GetProperty("OBSERVACOES").GetValue(query[i], null);
                            object INSERCAO = query[i].GetType().GetProperty("INSERCAO").GetValue(query[i], null);
                            object DATAINSERCAO = query[i].GetType().GetProperty("DATA_INSERCAO").GetValue(query[i], null);
                            object ACTUALIZACAO = query[i].GetType().GetProperty("ACTUALIZACAO").GetValue(query[i], null);
                            object DATAACTUALIZACAO = query[i].GetType().GetProperty("DATA_ACTUALIZACAO").GetValue(query[i], null);

                            currentRow++;
                            //worksheet.Cell(currentRow, 1).Value = UTILIZADOR;
                            worksheet.Cell(currentRow, 1).Value = ATLETA != null ? ATLETA.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 2).Value = NUMERO != null ? NUMERO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 3).Value = TREINO != null ? TREINO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 4).Value = PERIODO != null ? PERIODO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 5).Value = DATAINI != null ? DATAINI.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 6).Value = DATAFIM != null ? DATAFIM.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 7).Value = OBSERVACAO != null ? OBSERVACAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 8).Value = INSERCAO != null ? INSERCAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 9).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 10).Value = ACTUALIZACAO != null ? ACTUALIZACAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 11).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty;
                        }

                        worksheet.Columns().AdjustToContents();
                        using (MemoryStream myMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(myMemoryStream);

                            // return memory stream as byte array
                            return myMemoryStream.ToArray();
                        }
                    }
                case "gtquest":
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var worksheet = wb.Worksheets.Add("Sample Sheet");
                        var currentRow = 1;

                        worksheet.Cell(currentRow, 1).Value = "ATLETA";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Value = "N° SÓCIO";
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 3).Value = "TREINO";
                        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 4).Value = "INSERÇÃO";
                        worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 5).Value = "DATA INSERÇÃO";
                        worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 6).Value = "ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 7).Value = "DATA ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                        worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        for (int i = 0; i < query.Count; i++)
                        {
                            object ATLETA = query[i].GetType().GetProperty("NOME_PROPIO").GetValue(query[i], null) + " " + query[i].GetType().GetProperty("APELIDO").GetValue(query[i], null);
                            object NUMERO = query[i].GetType().GetProperty("NUMERO").GetValue(query[i], null);
                            object TREINO = query[i].GetType().GetProperty("treino").GetValue(query[i], null);
                            object INSERCAO = query[i].GetType().GetProperty("INSERCAO").GetValue(query[i], null);
                            object DATAINSERCAO = query[i].GetType().GetProperty("DATA_INSERCAO").GetValue(query[i], null);
                            object ACTUALIZACAO = query[i].GetType().GetProperty("ACTUALIZACAO").GetValue(query[i], null);
                            object DATAACTUALIZACAO = query[i].GetType().GetProperty("DATA_ACTUALIZACAO").GetValue(query[i], null);

                            currentRow++;
                            //worksheet.Cell(currentRow, 1).Value = UTILIZADOR;
                            worksheet.Cell(currentRow, 1).Value = ATLETA != null ? ATLETA.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 2).Value = NUMERO != null ? NUMERO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 3).Value = TREINO != null ? TREINO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 4).Value = INSERCAO != null ? INSERCAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 5).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 6).Value = ACTUALIZACAO != null ? ACTUALIZACAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 7).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty;
                        }

                        worksheet.Columns().AdjustToContents();
                        using (MemoryStream myMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(myMemoryStream);

                            // return memory stream as byte array
                            return myMemoryStream.ToArray();
                        }
                    }
                case "gtsearch":
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var worksheet = wb.Worksheets.Add("Sample Sheet");
                        var currentRow = 1;

                        worksheet.Cell(currentRow, 1).Value = "ATLETA";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Value = "N° SÓCIO";
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 3).Value = "ALTURA";
                        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 4).Value = "PESO";
                        worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 5).Value = "SEXO";
                        worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 6).Value = "INÍCIO DO PLANO / DATA AVALIAÇÃO";
                        worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 7).Value = "TREINO";
                        worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 8).Value = "AVALIAÇÃO";
                        worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 9).Value = "PERCENTIL";
                        worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
                        worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        for (int i = 0; i < query.Count; i++)
                        {
                            object ATLETA = query[i].GetType().GetProperty("NOME").GetValue(query[i], null);
                            object NUMERO = query[i].GetType().GetProperty("N_SOCIO").GetValue(query[i], null);
                            object ALTURA = query[i].GetType().GetProperty("ALTURA").GetValue(query[i], null);
                            object PESO = query[i].GetType().GetProperty("PESO").GetValue(query[i], null);
                            object SEXO = query[i].GetType().GetProperty("SEXO").GetValue(query[i], null);
                            object DATA = query[i].GetType().GetProperty("DATA_DEFAULT").GetValue(query[i], null);
                            object TREINO = query[i].GetType().GetProperty("TIPO_PLANO").GetValue(query[i], null);
                            object AVALIACAO = query[i].GetType().GetProperty("AVALIACAO").GetValue(query[i], null);
                            object PERCENTIL = query[i].GetType().GetProperty("PERCENTIL").GetValue(query[i], null);
                           
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = ATLETA != null ? ATLETA.ToString() : string.Empty; ;
                            worksheet.Cell(currentRow, 2).Value = NUMERO != null ? NUMERO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 3).Value = ALTURA != null ? ALTURA.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 4).Value = PESO != null ? PESO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 5).Value = SEXO != null ? SEXO.ToString() : string.Empty; ;
                            worksheet.Cell(currentRow, 6).Value = DATA != null ? DATA.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 7).Value = TREINO != null ? TREINO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 8).Value = AVALIACAO != null ? AVALIACAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 9).Value = PERCENTIL != null ? PERCENTIL.ToString() : string.Empty;
                         }

                        worksheet.Columns().AdjustToContents();
                        using (MemoryStream myMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(myMemoryStream);

                            // return memory stream as byte array
                            return myMemoryStream.ToArray();
                        }
                    }
                case "gtexercise":
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var worksheet = wb.Worksheets.Add("Sample Sheet");
                        var currentRow = 1;

                        worksheet.Cell(currentRow, 1).Value = "TREINO";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Value = "NOME";
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 3).Value = "ALONGAMENTO";
                        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 4).Value = "SEQUÊNCIA";
                        worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                        worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        for (int i = 0; i < query.Count; i++)
                        {
                            object TREINO = query[i].GetType().GetProperty("tr_nome").GetValue(query[i], null);
                            object NOME = query[i].GetType().GetProperty("nome").GetValue(query[i], null);
                            object ALONGAMENTO = query[i].GetType().GetProperty("ALONGAMENTO").GetValue(query[i], null);
                            object SEQUENCIA = query[i].GetType().GetProperty("SEQUENCIA").GetValue(query[i], null);
                           
                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = TREINO != null ? TREINO.ToString() : string.Empty; ;
                            worksheet.Cell(currentRow, 2).Value = NOME != null ? NOME.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 3).Value = ALONGAMENTO != null ? ALONGAMENTO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 4).Value = SEQUENCIA != null ? SEQUENCIA.ToString() : string.Empty;
                        }

                        worksheet.Columns().AdjustToContents();
                        using (MemoryStream myMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(myMemoryStream);

                            // return memory stream as byte array
                            return myMemoryStream.ToArray();
                        }
                    }
                case "GetUser":
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var worksheet = wb.Worksheets.Add("Sample Sheet");
                        var currentRow = 1;
                        worksheet.Cell(currentRow, 1).Value = "UTILIZADOR";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Value = "NOME";
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 3).Value = "ESTADO";
                        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 4).Value = "VALIDADO";
                        worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 5).Value = "DATA DE ACTIVAÇÃO";
                        worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 6).Value = "DATA DE DESACTIVAÇÃO";
                        worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 7).Value = "GRUPOS";
                        worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 8).Value = "PERFIS";
                        worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 9).Value = "ÚLTIMO INÍCIO DE SESSÃO";
                        worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 10).Value = "INSERÇÃO";
                        worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 11).Value = "DATA INSERÇÃO";
                        worksheet.Cell(currentRow, 11).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 12).Value = "ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 12).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 13).Value = "DATA ACTUALIZAÇÃO";
                        worksheet.Cell(currentRow, 13).Style.Font.Bold = true;
                        worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        for (int i = 0; i < query.Count; i++)
                        {
                            object UTILIZADOR = query[i].GetType().GetProperty("LOGIN").GetValue(query[i], null);
                            object NOME = query[i].GetType().GetProperty("NOME").GetValue(query[i], null);
                            object ESTADO = query[i].GetType().GetProperty("ACTIVO").GetValue(query[i], null);
                            object VALIDA = query[i].GetType().GetProperty("CONTA_VALIDA").GetValue(query[i], null);
                            object DATAACTIVACAO = query[i].GetType().GetProperty("DATA_ACT").GetValue(query[i], null);
                            object DATADESACTIVACAO = query[i].GetType().GetProperty("DATA_DESACT").GetValue(query[i], null);
                            //object ACESSOOAUTH = query[i].GetType().GetProperty("OAUTH2").GetValue(query[i], null);
                            //object ACESSOLADP = query[i].GetType().GetProperty("LADP").GetValue(query[i], null);
                            object GRUPOS = query[i].GetType().GetProperty("TOTALGROUPS").GetValue(query[i], null);
                            object PERFIS = query[i].GetType().GetProperty("TOTALPERFIS").GetValue(query[i], null);
                            object ULTIMOINICIODESESSAO = query[i].GetType().GetProperty("ULTIMA_SESSAO").GetValue(query[i], null);
                            object INSERCAO = query[i].GetType().GetProperty("INSERCAO").GetValue(query[i], null);
                            object DATAINSERCAO = query[i].GetType().GetProperty("DATA_INSERCAO").GetValue(query[i], null);
                            object ACTUALIZACAO = query[i].GetType().GetProperty("ACTUALIZACAO").GetValue(query[i], null);
                            object DATAACTUALIZACAO = query[i].GetType().GetProperty("DATA_ACTUALIZACAO").GetValue(query[i], null);

                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = UTILIZADOR != null ? UTILIZADOR.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 2).Value = NOME != null ? NOME.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 3).Value = ESTADO != null ? ESTADO.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 4).Value = VALIDA != null ? VALIDA.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 5).Value = DATAACTIVACAO != null ? DATAACTIVACAO.ToString() : string.Empty ;
                            worksheet.Cell(currentRow, 6).Value = DATADESACTIVACAO != null ? DATADESACTIVACAO.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 7).Value = GRUPOS != null ? GRUPOS.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 8).Value = PERFIS != null ? PERFIS.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 9).Value = ULTIMOINICIODESESSAO != null ? ULTIMOINICIODESESSAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 10).Value = INSERCAO != null ? INSERCAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 11).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 12).Value = ACTUALIZACAO != null ? ACTUALIZACAO.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 13).Value = DATAINSERCAO != null ? DATAINSERCAO.ToString() : string.Empty;
                        }

                        worksheet.Columns().AdjustToContents();
                        using (MemoryStream myMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(myMemoryStream);

                            // return memory stream as byte array
                            return myMemoryStream.ToArray();
                        }
                    }
                case "GetUserLog":
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var worksheet = wb.Worksheets.Add("Sample Sheet");
                        var currentRow = 1;
                        worksheet.Cell(currentRow, 1).Value = "LOGIN";
                        worksheet.Cell(currentRow, 1).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 2).Value = "MÓDULO";
                        worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 3).Value = "ENDEREÇO IP";
                        worksheet.Cell(currentRow, 3).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 4).Value = "ENDEREÇO MAC";
                        worksheet.Cell(currentRow, 4).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 5).Value = "HOSPEDEIRO HOST";
                        worksheet.Cell(currentRow, 5).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 6).Value = "DISPOSITIVO";
                        worksheet.Cell(currentRow, 6).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 7).Value = "LATITUDE";
                        worksheet.Cell(currentRow, 7).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 8).Value = "LONGITUDE";
                        worksheet.Cell(currentRow, 8).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 9).Value = "URL";
                        worksheet.Cell(currentRow, 9).Style.Font.Bold = true;
                        worksheet.Cell(currentRow, 10).Value = "DATA";
                        worksheet.Cell(currentRow, 10).Style.Font.Bold = true;
                        worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                        for (int i = 0; i < query.Count; i++)
                        {
                            object LOGIN = query[i].GetType().GetProperty("LOGIN").GetValue(query[i], null);
                            //object MODULO = query[i].GetType().GetProperty("MODULO").GetValue(query[i], null);
                            object ENDERECOIP = query[i].GetType().GetProperty("ENDERECO_IP").GetValue(query[i], null);
                            object ENDERECOMAC = query[i].GetType().GetProperty("ENDERECO_MAC").GetValue(query[i], null);
                            object HOST = query[i].GetType().GetProperty("HOSPEDEIRO_HOST").GetValue(query[i], null);
                            object DISPOSITIVO = query[i].GetType().GetProperty("DISPOSITIVO").GetValue(query[i], null);
                            object LAT = query[i].GetType().GetProperty("LAT").GetValue(query[i], null);
                            object LONG = query[i].GetType().GetProperty("LONG").GetValue(query[i], null);
                            object URL = query[i].GetType().GetProperty("URL").GetValue(query[i], null);
                            object DATA = query[i].GetType().GetProperty("DATA").GetValue(query[i], null);

                            currentRow++;
                            worksheet.Cell(currentRow, 1).Value = LOGIN != null ? LOGIN.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 2).Value = "GESTREINO";
                            worksheet.Cell(currentRow, 3).Value = ENDERECOIP != null ? ENDERECOIP.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 4).Value = ENDERECOMAC != null ? ENDERECOMAC.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 5).Value = HOST != null ? HOST.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 6).Value = DISPOSITIVO != null ? DISPOSITIVO.ToString() : string.Empty; 
                            worksheet.Cell(currentRow, 7).Value = LAT != null ? LAT.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 8).Value = LONG != null ? LONG.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 9).Value = URL != null ? URL.ToString() : string.Empty;
                            worksheet.Cell(currentRow, 10).Value = DATA != null ? DATA.ToString() : string.Empty;
                        }

                        worksheet.Columns().AdjustToContents();
                        using (MemoryStream myMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(myMemoryStream);

                            // return memory stream as byte array
                            return myMemoryStream.ToArray();
                        }
                    }
                default:
                    return null;
            }
        }
    }
}