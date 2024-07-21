using FluentValidation;
using ProjectMarket.Server.Data.Validators;

namespace ProjectMarket.Server.Data.Model.VO;

public struct KnowledgeAreaVO {
    required public string KnowledgeAreaName { get; set; }

    public KnowledgeAreaVO(string name) {
        KnowledgeAreaName = name;

        var validator = new KnowledgeAreaValidator();
        validator.ValidateAndThrow(this);
    }
}