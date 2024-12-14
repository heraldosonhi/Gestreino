﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gestreino
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class GESTREINO_Entities : DbContext
    {
        public GESTREINO_Entities()
            : base("name=GESTREINO_Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<GRL_ANO_LECTIVO> GRL_ANO_LECTIVO { get; set; }
        public virtual DbSet<GRL_ARQUIVOS> GRL_ARQUIVOS { get; set; }
        public virtual DbSet<GRL_ARQUIVOS_TIPO_DOCS> GRL_ARQUIVOS_TIPO_DOCS { get; set; }
        public virtual DbSet<GRL_ENDERECO_CIDADE> GRL_ENDERECO_CIDADE { get; set; }
        public virtual DbSet<GRL_ENDERECO_MUN_DISTR> GRL_ENDERECO_MUN_DISTR { get; set; }
        public virtual DbSet<GRL_ENDERECO_PAIS> GRL_ENDERECO_PAIS { get; set; }
        public virtual DbSet<GRL_TOKENS> GRL_TOKENS { get; set; }
        public virtual DbSet<GRL_TOKENS_TIPOS> GRL_TOKENS_TIPOS { get; set; }
        public virtual DbSet<INST_APLICACAO_ARQUIVOS> INST_APLICACAO_ARQUIVOS { get; set; }
        public virtual DbSet<INST_APLICACAO_CONTACTOS> INST_APLICACAO_CONTACTOS { get; set; }
        public virtual DbSet<INST_APLICACAO_ENDERECOS> INST_APLICACAO_ENDERECOS { get; set; }
        public virtual DbSet<PES_ARQUIVOS> PES_ARQUIVOS { get; set; }
        public virtual DbSet<PES_CONTACTOS> PES_CONTACTOS { get; set; }
        public virtual DbSet<PES_ENDERECOS> PES_ENDERECOS { get; set; }
        public virtual DbSet<PES_ESTADO_CIVIL> PES_ESTADO_CIVIL { get; set; }
        public virtual DbSet<PES_FAMILIARES_GRUPOS> PES_FAMILIARES_GRUPOS { get; set; }
        public virtual DbSet<PES_IDENTIFICACAO> PES_IDENTIFICACAO { get; set; }
        public virtual DbSet<PES_IDENTIFICACAO_LOCAL_EM> PES_IDENTIFICACAO_LOCAL_EM { get; set; }
        public virtual DbSet<PES_NACIONALIDADE> PES_NACIONALIDADE { get; set; }
        public virtual DbSet<PES_NATURALIDADE> PES_NATURALIDADE { get; set; }
        public virtual DbSet<PES_PESSOAS_CARACT> PES_PESSOAS_CARACT { get; set; }
        public virtual DbSet<PES_PESSOAS_CARACT_DEFICIENCIA> PES_PESSOAS_CARACT_DEFICIENCIA { get; set; }
        public virtual DbSet<PES_PESSOAS_CARACT_GRAU_DEF> PES_PESSOAS_CARACT_GRAU_DEF { get; set; }
        public virtual DbSet<PES_PESSOAS_CARACT_TIPO_DEF> PES_PESSOAS_CARACT_TIPO_DEF { get; set; }
        public virtual DbSet<PES_PESSOAS_CARACT_TIPO_SANG> PES_PESSOAS_CARACT_TIPO_SANG { get; set; }
        public virtual DbSet<PES_PESSOAS_FAM> PES_PESSOAS_FAM { get; set; }
        public virtual DbSet<PES_PESSOAS_FAM_CONTACTOS> PES_PESSOAS_FAM_CONTACTOS { get; set; }
        public virtual DbSet<PES_PESSOAS_FAM_ENDERECOS> PES_PESSOAS_FAM_ENDERECOS { get; set; }
        public virtual DbSet<PES_PESSOAS_PROFISSOES> PES_PESSOAS_PROFISSOES { get; set; }
        public virtual DbSet<PES_PROFISSOES> PES_PROFISSOES { get; set; }
        public virtual DbSet<PES_PROFISSOES_REGIME_TRABALHO> PES_PROFISSOES_REGIME_TRABALHO { get; set; }
        public virtual DbSet<PES_PROFISSOES_TIPO_CONTRACTO> PES_PROFISSOES_TIPO_CONTRACTO { get; set; }
        public virtual DbSet<PES_TIPO_ENDERECOS> PES_TIPO_ENDERECOS { get; set; }
        public virtual DbSet<PES_TIPO_IDENTIFICACAO> PES_TIPO_IDENTIFICACAO { get; set; }
        public virtual DbSet<UTILIZADORES> UTILIZADORES { get; set; }
        public virtual DbSet<UTILIZADORES_ACESSO_ATOMOS_GRUPOS> UTILIZADORES_ACESSO_ATOMOS_GRUPOS { get; set; }
        public virtual DbSet<UTILIZADORES_ACESSO_GRUPOS> UTILIZADORES_ACESSO_GRUPOS { get; set; }
        public virtual DbSet<UTILIZADORES_ACESSO_PERFIS> UTILIZADORES_ACESSO_PERFIS { get; set; }
        public virtual DbSet<UTILIZADORES_ACESSO_PERFIS_ATOMOS> UTILIZADORES_ACESSO_PERFIS_ATOMOS { get; set; }
        public virtual DbSet<UTILIZADORES_ACESSO_UTIL_GRUPOS> UTILIZADORES_ACESSO_UTIL_GRUPOS { get; set; }
        public virtual DbSet<UTILIZADORES_ACESSO_UTIL_PERFIS> UTILIZADORES_ACESSO_UTIL_PERFIS { get; set; }
        public virtual DbSet<UTILIZADORES_LOGIN_LOGS> UTILIZADORES_LOGIN_LOGS { get; set; }
        public virtual DbSet<UTILIZADORES_LOGIN_PASSWORD_TENT> UTILIZADORES_LOGIN_PASSWORD_TENT { get; set; }
        public virtual DbSet<GRL_DEFINICOES> GRL_DEFINICOES { get; set; }
        public virtual DbSet<INST_APLICACAO> INST_APLICACAO { get; set; }
        public virtual DbSet<PES_PESSOAS> PES_PESSOAS { get; set; }
        public virtual DbSet<UTILIZADORES_ACESSO_ATOMOS> UTILIZADORES_ACESSO_ATOMOS { get; set; }
    
        public virtual ObjectResult<SP_UTILIZADORES_LOGIN_LOGS_Result> SP_UTILIZADORES_LOGIN_LOGS(Nullable<int> userId, string action)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var actionParameter = action != null ?
                new ObjectParameter("Action", action) :
                new ObjectParameter("Action", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_UTILIZADORES_LOGIN_LOGS_Result>("SP_UTILIZADORES_LOGIN_LOGS", userIdParameter, actionParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> SP_UTILIZADORES_LOGIN(Nullable<int> userId, Nullable<int> moduleId, string ipAddress, string macAddress, string host, string device, Nullable<decimal> lat, Nullable<decimal> @long, string url, string action)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var moduleIdParameter = moduleId.HasValue ?
                new ObjectParameter("ModuleId", moduleId) :
                new ObjectParameter("ModuleId", typeof(int));
    
            var ipAddressParameter = ipAddress != null ?
                new ObjectParameter("IpAddress", ipAddress) :
                new ObjectParameter("IpAddress", typeof(string));
    
            var macAddressParameter = macAddress != null ?
                new ObjectParameter("MacAddress", macAddress) :
                new ObjectParameter("MacAddress", typeof(string));
    
            var hostParameter = host != null ?
                new ObjectParameter("Host", host) :
                new ObjectParameter("Host", typeof(string));
    
            var deviceParameter = device != null ?
                new ObjectParameter("Device", device) :
                new ObjectParameter("Device", typeof(string));
    
            var latParameter = lat.HasValue ?
                new ObjectParameter("Lat", lat) :
                new ObjectParameter("Lat", typeof(decimal));
    
            var longParameter = @long.HasValue ?
                new ObjectParameter("Long", @long) :
                new ObjectParameter("Long", typeof(decimal));
    
            var urlParameter = url != null ?
                new ObjectParameter("Url", url) :
                new ObjectParameter("Url", typeof(string));
    
            var actionParameter = action != null ?
                new ObjectParameter("Action", action) :
                new ObjectParameter("Action", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("SP_UTILIZADORES_LOGIN", userIdParameter, moduleIdParameter, ipAddressParameter, macAddressParameter, hostParameter, deviceParameter, latParameter, longParameter, urlParameter, actionParameter);
        }
    
        public virtual ObjectResult<SP_PES_ENT_PESSOAS_Result> SP_PES_ENT_PESSOAS(Nullable<int> iD, string nome, string sexo, Nullable<System.DateTime> dataNascimento, Nullable<int> estadoCivilId, string nIF, string apresentacaoPessoal, Nullable<int> paisId, Nullable<int> cidadeId, Nullable<int> municipioId, Nullable<decimal> telefone, Nullable<decimal> telefoneAlternativo, Nullable<decimal> telefoneResidencial, Nullable<decimal> fax, string email, string codigoPostal, string url, Nullable<decimal> altura, Nullable<decimal> peso, Nullable<int> tipoSangueId, Nullable<int> userId, string action)
        {
            var iDParameter = iD.HasValue ?
                new ObjectParameter("ID", iD) :
                new ObjectParameter("ID", typeof(int));
    
            var nomeParameter = nome != null ?
                new ObjectParameter("Nome", nome) :
                new ObjectParameter("Nome", typeof(string));
    
            var sexoParameter = sexo != null ?
                new ObjectParameter("Sexo", sexo) :
                new ObjectParameter("Sexo", typeof(string));
    
            var dataNascimentoParameter = dataNascimento.HasValue ?
                new ObjectParameter("DataNascimento", dataNascimento) :
                new ObjectParameter("DataNascimento", typeof(System.DateTime));
    
            var estadoCivilIdParameter = estadoCivilId.HasValue ?
                new ObjectParameter("EstadoCivilId", estadoCivilId) :
                new ObjectParameter("EstadoCivilId", typeof(int));
    
            var nIFParameter = nIF != null ?
                new ObjectParameter("NIF", nIF) :
                new ObjectParameter("NIF", typeof(string));
    
            var apresentacaoPessoalParameter = apresentacaoPessoal != null ?
                new ObjectParameter("ApresentacaoPessoal", apresentacaoPessoal) :
                new ObjectParameter("ApresentacaoPessoal", typeof(string));
    
            var paisIdParameter = paisId.HasValue ?
                new ObjectParameter("PaisId", paisId) :
                new ObjectParameter("PaisId", typeof(int));
    
            var cidadeIdParameter = cidadeId.HasValue ?
                new ObjectParameter("CidadeId", cidadeId) :
                new ObjectParameter("CidadeId", typeof(int));
    
            var municipioIdParameter = municipioId.HasValue ?
                new ObjectParameter("MunicipioId", municipioId) :
                new ObjectParameter("MunicipioId", typeof(int));
    
            var telefoneParameter = telefone.HasValue ?
                new ObjectParameter("Telefone", telefone) :
                new ObjectParameter("Telefone", typeof(decimal));
    
            var telefoneAlternativoParameter = telefoneAlternativo.HasValue ?
                new ObjectParameter("TelefoneAlternativo", telefoneAlternativo) :
                new ObjectParameter("TelefoneAlternativo", typeof(decimal));
    
            var telefoneResidencialParameter = telefoneResidencial.HasValue ?
                new ObjectParameter("TelefoneResidencial", telefoneResidencial) :
                new ObjectParameter("TelefoneResidencial", typeof(decimal));
    
            var faxParameter = fax.HasValue ?
                new ObjectParameter("Fax", fax) :
                new ObjectParameter("Fax", typeof(decimal));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var codigoPostalParameter = codigoPostal != null ?
                new ObjectParameter("CodigoPostal", codigoPostal) :
                new ObjectParameter("CodigoPostal", typeof(string));
    
            var urlParameter = url != null ?
                new ObjectParameter("Url", url) :
                new ObjectParameter("Url", typeof(string));
    
            var alturaParameter = altura.HasValue ?
                new ObjectParameter("Altura", altura) :
                new ObjectParameter("Altura", typeof(decimal));
    
            var pesoParameter = peso.HasValue ?
                new ObjectParameter("Peso", peso) :
                new ObjectParameter("Peso", typeof(decimal));
    
            var tipoSangueIdParameter = tipoSangueId.HasValue ?
                new ObjectParameter("TipoSangueId", tipoSangueId) :
                new ObjectParameter("TipoSangueId", typeof(int));
    
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var actionParameter = action != null ?
                new ObjectParameter("Action", action) :
                new ObjectParameter("Action", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_PES_ENT_PESSOAS_Result>("SP_PES_ENT_PESSOAS", iDParameter, nomeParameter, sexoParameter, dataNascimentoParameter, estadoCivilIdParameter, nIFParameter, apresentacaoPessoalParameter, paisIdParameter, cidadeIdParameter, municipioIdParameter, telefoneParameter, telefoneAlternativoParameter, telefoneResidencialParameter, faxParameter, emailParameter, codigoPostalParameter, urlParameter, alturaParameter, pesoParameter, tipoSangueIdParameter, userIdParameter, actionParameter);
        }
    
        public virtual ObjectResult<SP_UTILIZADORES_ENT_UTILIZADORES_Result> SP_UTILIZADORES_ENT_UTILIZADORES(Nullable<int> id, Nullable<int> subGroupId, Nullable<int> profileId, string login, string nome, Nullable<decimal> telefone, string email, string senha, string salt, Nullable<bool> activo, Nullable<System.DateTime> dataAct, Nullable<System.DateTime> dataDesact, Nullable<bool> validada, Nullable<int> userInsercaoId, string action)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var subGroupIdParameter = subGroupId.HasValue ?
                new ObjectParameter("SubGroupId", subGroupId) :
                new ObjectParameter("SubGroupId", typeof(int));
    
            var profileIdParameter = profileId.HasValue ?
                new ObjectParameter("ProfileId", profileId) :
                new ObjectParameter("ProfileId", typeof(int));
    
            var loginParameter = login != null ?
                new ObjectParameter("login", login) :
                new ObjectParameter("login", typeof(string));
    
            var nomeParameter = nome != null ?
                new ObjectParameter("Nome", nome) :
                new ObjectParameter("Nome", typeof(string));
    
            var telefoneParameter = telefone.HasValue ?
                new ObjectParameter("Telefone", telefone) :
                new ObjectParameter("Telefone", typeof(decimal));
    
            var emailParameter = email != null ?
                new ObjectParameter("Email", email) :
                new ObjectParameter("Email", typeof(string));
    
            var senhaParameter = senha != null ?
                new ObjectParameter("Senha", senha) :
                new ObjectParameter("Senha", typeof(string));
    
            var saltParameter = salt != null ?
                new ObjectParameter("Salt", salt) :
                new ObjectParameter("Salt", typeof(string));
    
            var activoParameter = activo.HasValue ?
                new ObjectParameter("Activo", activo) :
                new ObjectParameter("Activo", typeof(bool));
    
            var dataActParameter = dataAct.HasValue ?
                new ObjectParameter("DataAct", dataAct) :
                new ObjectParameter("DataAct", typeof(System.DateTime));
    
            var dataDesactParameter = dataDesact.HasValue ?
                new ObjectParameter("DataDesact", dataDesact) :
                new ObjectParameter("DataDesact", typeof(System.DateTime));
    
            var validadaParameter = validada.HasValue ?
                new ObjectParameter("Validada", validada) :
                new ObjectParameter("Validada", typeof(bool));
    
            var userInsercaoIdParameter = userInsercaoId.HasValue ?
                new ObjectParameter("UserInsercaoId", userInsercaoId) :
                new ObjectParameter("UserInsercaoId", typeof(int));
    
            var actionParameter = action != null ?
                new ObjectParameter("Action", action) :
                new ObjectParameter("Action", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_UTILIZADORES_ENT_UTILIZADORES_Result>("SP_UTILIZADORES_ENT_UTILIZADORES", idParameter, subGroupIdParameter, profileIdParameter, loginParameter, nomeParameter, telefoneParameter, emailParameter, senhaParameter, saltParameter, activoParameter, dataActParameter, dataDesactParameter, validadaParameter, userInsercaoIdParameter, actionParameter);
        }
    
        public virtual ObjectResult<SP_UTILIZADORES_ENT_ATOMOS_Result> SP_UTILIZADORES_ENT_ATOMOS(Nullable<int> id, string nome, string descricao, Nullable<int> userInsercaoId, string action)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var nomeParameter = nome != null ?
                new ObjectParameter("Nome", nome) :
                new ObjectParameter("Nome", typeof(string));
    
            var descricaoParameter = descricao != null ?
                new ObjectParameter("Descricao", descricao) :
                new ObjectParameter("Descricao", typeof(string));
    
            var userInsercaoIdParameter = userInsercaoId.HasValue ?
                new ObjectParameter("UserInsercaoId", userInsercaoId) :
                new ObjectParameter("UserInsercaoId", typeof(int));
    
            var actionParameter = action != null ?
                new ObjectParameter("Action", action) :
                new ObjectParameter("Action", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_UTILIZADORES_ENT_ATOMOS_Result>("SP_UTILIZADORES_ENT_ATOMOS", idParameter, nomeParameter, descricaoParameter, userInsercaoIdParameter, actionParameter);
        }
    
        public virtual ObjectResult<SP_UTILIZADORES_ENT_GRUPOS_Result> SP_UTILIZADORES_ENT_GRUPOS(Nullable<int> id, string sigla, string nome, string descricao, Nullable<int> userInsercaoId, string action)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var siglaParameter = sigla != null ?
                new ObjectParameter("Sigla", sigla) :
                new ObjectParameter("Sigla", typeof(string));
    
            var nomeParameter = nome != null ?
                new ObjectParameter("Nome", nome) :
                new ObjectParameter("Nome", typeof(string));
    
            var descricaoParameter = descricao != null ?
                new ObjectParameter("Descricao", descricao) :
                new ObjectParameter("Descricao", typeof(string));
    
            var userInsercaoIdParameter = userInsercaoId.HasValue ?
                new ObjectParameter("UserInsercaoId", userInsercaoId) :
                new ObjectParameter("UserInsercaoId", typeof(int));
    
            var actionParameter = action != null ?
                new ObjectParameter("Action", action) :
                new ObjectParameter("Action", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_UTILIZADORES_ENT_GRUPOS_Result>("SP_UTILIZADORES_ENT_GRUPOS", idParameter, siglaParameter, nomeParameter, descricaoParameter, userInsercaoIdParameter, actionParameter);
        }
    
        public virtual ObjectResult<SP_UTILIZADORES_ENT_PERFIS_Result> SP_UTILIZADORES_ENT_PERFIS(Nullable<int> id, string nome, string descricao, Nullable<int> userInsercaoId, string action)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var nomeParameter = nome != null ?
                new ObjectParameter("Nome", nome) :
                new ObjectParameter("Nome", typeof(string));
    
            var descricaoParameter = descricao != null ?
                new ObjectParameter("Descricao", descricao) :
                new ObjectParameter("Descricao", typeof(string));
    
            var userInsercaoIdParameter = userInsercaoId.HasValue ?
                new ObjectParameter("UserInsercaoId", userInsercaoId) :
                new ObjectParameter("UserInsercaoId", typeof(int));
    
            var actionParameter = action != null ?
                new ObjectParameter("Action", action) :
                new ObjectParameter("Action", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_UTILIZADORES_ENT_PERFIS_Result>("SP_UTILIZADORES_ENT_PERFIS", idParameter, nomeParameter, descricaoParameter, userInsercaoIdParameter, actionParameter);
        }
    
        public virtual ObjectResult<SP_UTILIZADORES_ENT_ATOMOS_GRUPOS_Result> SP_UTILIZADORES_ENT_ATOMOS_GRUPOS(Nullable<int> id, Nullable<int> groupId, Nullable<int> atomId, Nullable<int> userInsercaoId, string action)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var groupIdParameter = groupId.HasValue ?
                new ObjectParameter("GroupId", groupId) :
                new ObjectParameter("GroupId", typeof(int));
    
            var atomIdParameter = atomId.HasValue ?
                new ObjectParameter("AtomId", atomId) :
                new ObjectParameter("AtomId", typeof(int));
    
            var userInsercaoIdParameter = userInsercaoId.HasValue ?
                new ObjectParameter("UserInsercaoId", userInsercaoId) :
                new ObjectParameter("UserInsercaoId", typeof(int));
    
            var actionParameter = action != null ?
                new ObjectParameter("Action", action) :
                new ObjectParameter("Action", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_UTILIZADORES_ENT_ATOMOS_GRUPOS_Result>("SP_UTILIZADORES_ENT_ATOMOS_GRUPOS", idParameter, groupIdParameter, atomIdParameter, userInsercaoIdParameter, actionParameter);
        }
    
        public virtual ObjectResult<SP_UTILIZADORES_ENT_GRUPOS_UTILIZADORES_Result> SP_UTILIZADORES_ENT_GRUPOS_UTILIZADORES(Nullable<int> id, Nullable<int> groupId, Nullable<int> userId, Nullable<int> userInsercaoId, string action)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var groupIdParameter = groupId.HasValue ?
                new ObjectParameter("GroupId", groupId) :
                new ObjectParameter("GroupId", typeof(int));
    
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var userInsercaoIdParameter = userInsercaoId.HasValue ?
                new ObjectParameter("UserInsercaoId", userInsercaoId) :
                new ObjectParameter("UserInsercaoId", typeof(int));
    
            var actionParameter = action != null ?
                new ObjectParameter("Action", action) :
                new ObjectParameter("Action", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_UTILIZADORES_ENT_GRUPOS_UTILIZADORES_Result>("SP_UTILIZADORES_ENT_GRUPOS_UTILIZADORES", idParameter, groupIdParameter, userIdParameter, userInsercaoIdParameter, actionParameter);
        }
    
        public virtual ObjectResult<SP_UTILIZADORES_ENT_PERFIS_ATOMOS_Result> SP_UTILIZADORES_ENT_PERFIS_ATOMOS(Nullable<int> id, Nullable<int> profileId, Nullable<int> atomId, Nullable<int> userInsercaoId, string action)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var profileIdParameter = profileId.HasValue ?
                new ObjectParameter("ProfileId", profileId) :
                new ObjectParameter("ProfileId", typeof(int));
    
            var atomIdParameter = atomId.HasValue ?
                new ObjectParameter("AtomId", atomId) :
                new ObjectParameter("AtomId", typeof(int));
    
            var userInsercaoIdParameter = userInsercaoId.HasValue ?
                new ObjectParameter("UserInsercaoId", userInsercaoId) :
                new ObjectParameter("UserInsercaoId", typeof(int));
    
            var actionParameter = action != null ?
                new ObjectParameter("Action", action) :
                new ObjectParameter("Action", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_UTILIZADORES_ENT_PERFIS_ATOMOS_Result>("SP_UTILIZADORES_ENT_PERFIS_ATOMOS", idParameter, profileIdParameter, atomIdParameter, userInsercaoIdParameter, actionParameter);
        }
    
        public virtual ObjectResult<SP_UTILIZADORES_ENT_UTILIZADORES_PERFIS_Result> SP_UTILIZADORES_ENT_UTILIZADORES_PERFIS(Nullable<int> id, Nullable<int> profileId, Nullable<int> userId, Nullable<int> userInsercaoId, string action)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            var profileIdParameter = profileId.HasValue ?
                new ObjectParameter("ProfileId", profileId) :
                new ObjectParameter("ProfileId", typeof(int));
    
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));
    
            var userInsercaoIdParameter = userInsercaoId.HasValue ?
                new ObjectParameter("UserInsercaoId", userInsercaoId) :
                new ObjectParameter("UserInsercaoId", typeof(int));
    
            var actionParameter = action != null ?
                new ObjectParameter("Action", action) :
                new ObjectParameter("Action", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_UTILIZADORES_ENT_UTILIZADORES_PERFIS_Result>("SP_UTILIZADORES_ENT_UTILIZADORES_PERFIS", idParameter, profileIdParameter, userIdParameter, userInsercaoIdParameter, actionParameter);
        }
    }
}
