using Dapper;
using ProjectMarket.Server.Data.Model.ValueObjects;
using ProjectMarket.Server.Infra.Db;
using SqlKata.Compilers;

namespace ProjectMarket.Server.Infra.Repository;

public class KnowledgeAreaRepository(IUnitOfWork unitOfWork, Compiler compiler)
{
    public readonly IUnitOfWork UnitOfWork = unitOfWork;
    
    public IEnumerable<KnowledgeAreaVo> GetAll()
    {
        // TODO Use pagination instead.
        const string sql = """
                           SELECT "KnowledgeAreaName"
                           FROM "KnowledgeArea"
                           """;
        return UnitOfWork.Connection.Query<KnowledgeAreaVo>(sql);
    }

    public KnowledgeAreaVo GetKnowledgeAreaByName(string name)
    {
        const string sql = """
                           SELECT "KnowledgeAreaName" 
                           FROM "KnowledgeArea" 
                           WHERE "KnowledgeAreaName" = @KnowledgeAreaName
                           """;
        try
        {
            var record = UnitOfWork.Connection.QuerySingle<KnowledgeAreaRecord>(sql, new { KnowledgeAreaName = name });
            KnowledgeAreaVo result = new(record);
            return result;
        }
        catch (Exception)
        {
            throw new ArgumentException($"{nameof(KnowledgeAreaVo.KnowledgeAreaName)} \'{name}\' not found");
        }
    }

    public KnowledgeAreaVo Insert(KnowledgeAreaVo knowledgeArea)
    {
        const string sql = """
                           INSERT INTO "KnowledgeArea" ("KnowledgeAreaName") 
                           VALUES (@KnowledgeAreaName) 
                           RETURNING "KnowledgeAreaName"
                           """;
        return UnitOfWork.Connection.QuerySingle<KnowledgeAreaVo>(sql, knowledgeArea);
    }

    public bool Update(string name, KnowledgeAreaVo knowledgeArea)
    {
        const string sql = """
                           UPDATE "KnowledgeArea" 
                           SET "KnowledgeAreaName" = @KnowledgeAreaName 
                           WHERE "KnowledgeAreaName" = @KnowledgeAreaNameToUpdate 
                           RETURNING true
                           """;
        return UnitOfWork.Connection.QuerySingle<bool>(sql, new
        {
            KnowledgeAreaNameToUpdate = name,
            KnowledgeAreaName = knowledgeArea.KnowledgeAreaName
        });
    }

    public bool Delete(string name)
    {
        const string sql = """
                           DELETE FROM "KnowledgeArea" CASCADE 
                           WHERE "KnowledgeAreaName" = @KnowledgeAreaName 
                           RETURNING true
                           """;
        return UnitOfWork.Connection.QuerySingle<bool>(sql, new { KnowledgeAreaName = name });
    }
}