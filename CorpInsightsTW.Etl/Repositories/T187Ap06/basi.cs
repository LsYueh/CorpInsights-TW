using CorpInsightsTW.Etl.Dtos.T187Ap06;

namespace CorpInsightsTW.Etl.Repositories.T187Ap06;

public class BasiRepository(string connectionString) : T187Repository<BasiDto>(connectionString)
{
    protected override string Taxonomy => "basi";

    protected override string MainTableUpsertSql => @"
        INSERT INTO t187ap06_basi (
            year,
            quarter,
            listing_status,
            company_code,
            company_name,
            interest_income,
            non_interest_income,
            provisions_expenses,
            operating_expenses,
            income_before_tax,
            income_tax,
            income_after_tax,
            discontinued_ops_income,
            pre_merger_non_control_income,
            net_income,
            other_comprehensive_income,
            pre_merger_non_control_oci,
            total_comprehensive_income,
            net_income_parent,
            net_income_predecessor,
            net_income_nci,
            comp_income_parent,
            comp_income_predecessor,
            comp_income_nci,
            basic_eps,
            updated_at
        ) VALUES (
            @Year,
            @Quarter,
            @ListingStatus,
            @CompanyCode,
            @CompanyName,
            @InterestIncome,
            @NonInterestIncome,
            @ProvisionsExpenses,
            @OperatingExpenses,
            @IncomeBeforeTax,
            @IncomeTax,
            @IncomeAfterTax,
            @DiscontinuedOpsIncome,
            @PreMergerNonControlIncome,
            @NetIncome,
            @OtherComprehensiveIncome,
            @PreMergerNonControlOci,
            @TotalComprehensiveIncome,
            @NetIncomeParent,
            @NetIncomePredecessor,
            @NetIncomeNci,
            @CompIncomeParent,
            @CompIncomePredecessor,
            @CompIncomeNci,
            @BasicEps,
            NOW()
        )
        ON DUPLICATE KEY UPDATE
            listing_status                = VALUES(listing_status),
            company_name                  = VALUES(company_name),
            interest_income               = VALUES(interest_income),
            non_interest_income           = VALUES(non_interest_income),
            provisions_expenses           = VALUES(provisions_expenses),
            operating_expenses            = VALUES(operating_expenses),
            income_before_tax             = VALUES(income_before_tax),
            income_tax                    = VALUES(income_tax),
            income_after_tax              = VALUES(income_after_tax),
            discontinued_ops_income       = VALUES(discontinued_ops_income),
            pre_merger_non_control_income = VALUES(pre_merger_non_control_income),
            net_income                    = VALUES(net_income),
            other_comprehensive_income    = VALUES(other_comprehensive_income),
            pre_merger_non_control_oci    = VALUES(pre_merger_non_control_oci),
            total_comprehensive_income    = VALUES(total_comprehensive_income),
            net_income_parent             = VALUES(net_income_parent),
            net_income_predecessor        = VALUES(net_income_predecessor),
            net_income_nci                = VALUES(net_income_nci),
            comp_income_parent            = VALUES(comp_income_parent),
            comp_income_predecessor       = VALUES(comp_income_predecessor),
            comp_income_nci               = VALUES(comp_income_nci),
            basic_eps                     = VALUES(basic_eps),
            updated_at                    = NOW();";
}