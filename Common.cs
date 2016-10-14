using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.ServiceModel.Channels;
using System.Xml.Linq;
using Azur.XIFramework.Common;
using System.Xml;
using System.ServiceModel;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.Reporting.WebForms;

using System.Net;
using System.Net.Mail;

using System.Web;
using System.Web.Security;
using System.Web.Services;
using System.Web.Services.Protocols;

using System.Text;

using Microsoft.SqlServer.ReportingServices2010;
using System.Diagnostics;

using OfficeOpenXml;
using System.Linq;
using Azur.XIFramework;
using System.Configuration;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Core;

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// <summary>
/// This class contains the common enums, constants, and functions shared by the different server-side modules.
/// </summary>
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace IATA.RS.Common
{

    #region " Public Enums "

    /// <summary>
    /// An enumeration of the unique class identifiers. There is one identifier per application
    /// </summary>
    public enum ClassID : long
    {
        ClientBalances,
        Clawbacks,
        CompensationMngt,
        SettlementAdjustments,
        ManualPayments,
        SalesForceInfos,
        Settlement,
		HAR,
        TopmanScreenShots,
        ControlResults,
        Matching,
        ExpectedMovement,
        Reports,
        ShortNonPaymentMngt,
        Prorations,
        ProrationPlans,
        ProrationToClawbacks,
        Countries,
        Banks,
        Currencies,
        Sections,
        HeadOffices,
        Clients,
        SectionRoles,
        Notifications,
        Periods,
        Workflow,
        WorkflowActivity,
        WorkflowInstance,
        WorkflowActivityInstance,
        WorkflowActivityRights,
        UserSection,
        CalendarEvent,
        AgentInfoFiles,
        BankStatementFiles,
        BillingFiles,
        BillingTransactions,
        ImportationLogs,
        BankMovements,
        UntraceableSwiftFiles,
        ActivityLog,
        FieldChangeTracking,
        PaymentInstructionFiles,
        AdjustmentFiles,
        ICCSReports,
        AccountInfo,
        MessageStandardFields,
        PickLists,
        PickListValues,
        BillingAdjustments,

        // These classIDs are "Virtual"
        // The three refer to the "Client" class that is split between three
        Agencies,
        Airlines,
        OtherClients,
        SalesForceSIDRACases,
        ExternalMovementMngt,

        //Discrepancy ClassID
        RemDescReports,

        //SWIFT Directories
        IBANStructure,
        BankDirectoryPlus,
    }

    /// <summary>
    /// The type of Find operation to be sent to the server
    /// </summary>
    public enum FindType : long
    {
        FindByID,
        FindByCriteria,
        FindByKeyword,
        FindForGridByKeyword,
        FindForGridByCriteria
    }

    /// <summary>
    /// An enumeration of the unique association identifiers. There is one identifier per related entity
    /// </summary>
    public enum AssociationID
    {
        ClientBalances_ClientBalancesActions,
        ManualPayments_ManualPaymentDetails,
        Settlement_SettlementDetails,
        Settlement_SettlementAdjustments,
        Prorations_ProrationExclusions,
        Prorations_AssocProrationPlans,
        Prorations_ProrationToWorkflowInstance,
        ProrationPlans_ProrationClawbacks,
        Banks_BanksMessageConfiguration,
        Sections_SectionPeriods,
        Sections_SectionControlActivation,
        Sections_SectionNotifications,
        Sections_SectionBankAccounts,
        Sections_SectionBankingConfig,
        Sections_SectionAgentCodes,
        Sections_SectionRoles,
        Sections_SectionAutoMatchingRules,
        Sections_SectionHubBankingData,

        Sections_SectionHubSharePointAccessConfiguration,

        Sections_SectionHubICCS,
        Sections_SectionHubBSPLink,
        Sections_SectionHubCASSLink,
        Sections_SectionHubSIDRA,
        Sections_SectionHubCassAssoc,
        Sections_SectionHubBankConnect,
        Clients_ClientsBankingInformation,
        Clients_ClientsShortNames,
        SectionRoles_SectionRolesUsers,
        SectionRoles_SectionRolesRights,
        WorkflowActivity_PreviousActivities,
        WorkflowActivityInstance_NotificationInstance,
        AgentInfoFiles_AgentInfoFilesImportLogs,
        BankStatementFiles_BankStatementParts,
        BankStatementFiles_BankStatementFilesBankMov,
        BankStatementFiles_ImportationLogs,
        BillingFiles_BillingFilesTrans,
        BillingFiles_BillingFilesImportLogs,
        BillingFiles_AdjustmentsReport,
        BillingFiles_TransactionWarnings,
        PaymentInstructionFiles_PaymentAssociatedFiles,
        PaymentInstructionFiles_PaymentInstructions,
        AdjustmentFiles_AdjustmentFilesDetails,
        Matching_BillingTransactions,

        HAR_HARSupportingDocument,

        Settlement_SettlementSignatories,

        ManualPayments_ManualPaymentsSignatories,
      
        // Custom
        ClientBalances_ClientBalancesDetails,
        BankMovements_ManualMatching,
        PickListValues_PickListValuesDescription,
        PickLists_PickListsDescription,
        PickLists_PickListValues,
        Sections_SectionActivityDeadlines,
        Sections_SectionBankingMsgConfig,
        Sections_SectionPain001BR,
        Sections_SectionPain008BR,
        Sections_SectionMT101BR,
        Sections_SectionMT104BR,

        Sections_SectionHubRemDescReports,
    }

    /// <summary>
    /// An enumeration of the  unique formation association identifiers. There is one identifier for each OneToMany type related entity
    /// </summary>
    public enum FormattedAssociationID
    {
        ClientBalances_GetClientBalancesActions,
        ManualPayments_GetManualPaymentDetails,
        Settlement_GetSettlementDetails,
        Settlement_GetSettlementAdjustments,
        Prorations_GetProrationExclusions,
        Prorations_GetAssocProrationPlans,
        Prorations_GetProrationToWorkflowInstance,
        ProrationPlans_GetProrationClawbacks,
        Banks_GetBanksMessageConfiguration,
        Sections_GetSectionPeriods,
        Sections_GetSectionControlActivation,
        Sections_GetSectionNotifications,
        Sections_GetSectionBankAccounts,
        Sections_GetSectionAgentCodes,
        Sections_GetSectionRoles,
        Sections_GetSectionAutoMatchingRules,
        Sections_GetSectionHubCassAssoc,
        Clients_GetClientsBankingInformation,
        Clients_GetClientsShortNames,
        SectionRoles_GetSectionRolesUsers,
        SectionRoles_GetSectionRolesRights,
        WorkflowActivity_GetPreviousActivities,
        WorkflowActivityInstance_GetNotificationInstance,
        AgentInfoFiles_GetAgentInfoFilesImportLogs,
        BankStatementFiles_GetBankStatementParts,
        BankStatementFiles_GetBankStatementFilesBankMov,
        BankStatementFiles_GetImportationLogs,
        BillingFiles_GetBillingFilesTrans,
        BillingFiles_GetBillingFilesImportLogs,
        BillingFiles_GetTransactionWarnings,
        BillingFiles_GetAdjustmentsReport,
        PaymentInstructionFiles_GetPaymentAssociatedFiles,
        PaymentInstructionFiles_GetPaymentInstructions,
        AdjustmentFiles_GetAdjustmentFilesDetails,
        Matching_GetBillingTransactions,

        HAR_GetHARSupportingDocument,

        Settlement_GetSettlementSignatories,

        ManualPayments_GetManualPaymentsSignatories,
      
        //Custom
        ClientBalances_GetBalanceDetails,
        BankMovements_GetMatching,
        BankMovements_GetManualMatching,
        BillingFiles_GetBillingFilesTransByHO,
        PickListValues_GetPickListValuesDescription,
        PickLists_GetPickListsDescription,
        PickLists_GetPickListValues,
        Sections_GetSectionActivityDeadlines,
        Sections_GetSectionBankingMsgConfig,
    }

    public enum ConnectionID : long
    {
        Default = 0
    }

    public enum ProcedureID
    {
        None = 0
    }

    /// <summary>
    /// An enumeration of the unique ReadComplex identifiers.
    /// The ReadForEntityRelations (for the OWPASearchBox control) and the ReadFileContent (for the OWPAImageControl and the OWPAFileLink)
    /// operations have identifiers, as well as any other custom Read operation that needs to be added.
    /// </summary>
    public enum ReadComplexID
    {
        ClientBalances_ReadForEntityRelations,
        Settlement_ReadForEntityRelations,
        ExpectedMovement_ReadForEntityRelations,
        Prorations_ReadForEntityRelations,
        Prorations_ReadForEntityRelations_AgentLatePayment,
        ProrationPlans_ReadForEntityRelations,
        Countries_ReadForEntityRelations,
        Banks_ReadForEntityRelations,
        Currencies_ReadForEntityRelations,
        Sections_ReadForEntityRelations,
        HeadOffices_ReadForEntityRelations,
        Clients_ReadForEntityRelations,
        SectionRoles_ReadForEntityRelations,
        Notifications_ReadForEntityRelations,
        Periods_ReadForEntityRelations,
        Workflow_ReadForEntityRelations,
        WorkflowActivity_ReadForEntityRelations,
        WorkflowActivity_ReadForEntityRelationsPeriodLevelOnly,
        WorkflowInstance_ReadForEntityRelations,
        WorkflowActivityInstance_ReadForEntityRelations,
        AgentInfoFiles_ReadForEntityRelations,
        BankStatementFiles_ReadForEntityRelations,
        BillingFiles_ReadForEntityRelations,
        BillingTransactions_ReadForEntityRelations,
        ActivityLog_ReadForEntityRelations,
        AdjustmentFiles_ReadForEntityRelations,
        TopmanScreenShots_ReadComplex_ReadContentFileContent,
        ClientBalances_ReadComplex_ReadByPeriodID,
        ClientBalances_ReadComplex_ReadByClientID,
        ClientBalances_ReadComplex_ReadByStatusID,
        ClientBalances_ReadComplex_ReadBySalesforceReference,
        ClientBalances_ReadComplex_ReadByApprovalStatusID,
        ClientBalances_ReadComplex_ReadByLastUnapprovedModification,
        ClientBalances_ReadComplex_ReadByRequiresCaseUpdate,
        ClientBalances_ReadComplex_ClientBalancesActions_ReadByField1,
        CompensationMngt_ReadComplex_ReadByClientID,
        SettlementAdjustments_ReadComplex_ReadByAdjustmentTypeID,
        ManualPayments_ReadComplex_ManualPaymentDetails_ReadByClient,
        ManualPayments_ReadForEntityRelations,
        SalesForceInfos_ReadComplex_ReadByExternalID,
        Settlement_ReadComplex_ReadByWorkflowInstanceID,
        Settlement_ReadComplex_ReadByStatusID,
        Settlement_ReadComplex_ReadByTypeID,
        Settlement_ReadComplex_SettlementDetails_ReadByClientBalanceID,
        Settlement_ReadComplex_SettlementAdjustments_ReadByField1,
        Settlement_ReadComplex_SettlementHAR_ReadByTopmanScreenIncluded,
        TopmanScreenShots_ReadComplex_ReadByFileName,
        ControlResults_ReadComplex_ReadByWorkflowActivityInstanceID,
        Matching_ReadComplex_ReadByWorkflowInstanceID,
        Matching_ReadComplex_ReadByMatchingRuleTypeID,
        Reports_ReadComplex_ReadByName,
        ShortNonPaymentMngt_ReadComplex_ReadByInclusionStatusID,
        ShortNonPaymentMngt_ReadComplex_ReadBySalesforceReference,
        Countries_ReadComplex_ReadByCountryName,
        Countries_ReadComplex_ReadByLanguageID,
        Countries_ReadComplex_ReadByISOC,
        Banks_ReadComplex_ReadByName,
        Banks_ReadComplex_ReadByTypeID,
        Banks_ReadComplex_BanksMessageConfiguration_ReadByMessageTypeID,
        Currencies_ReadComplex_ReadByCurrencyCode,
        Sections_ReadComplex_ReadBySectionCode,
        Sections_ReadComplex_ReadByCountryID,
        Sections_ReadComplex_ReadBySectionName,
        Sections_ReadComplex_ReadBySystemTypeID,
        Sections_ReadComplex_SectionPeriods_ReadByField1,
        Sections_ReadComplex_SectionControlActivation_ReadByWorkflowActivityID,
        Sections_ReadComplex_SectionBankingConfig_ReadByCountryCode57D,
        Sections_ReadComplex_SectionRoles_ReadByField1,
        Sections_ReadComplex_SectionAutoMatchingRules_ReadByMatchOn,
        Sections_ReadComplex_SectionHubBankingData_ReadByBranchCode,
        HeadOffices_ReadComplex_ReadByHOCode,
        HeadOffices_ReadComplex_ReadByShortName,
        HeadOffices_ReadComplex_ReadByClientTypeID,
        HeadOffices_ReadComplex_ReadBySettlementTypeID,
        Clients_ReadComplex_ReadByClientCode,
        Clients_ReadComplex_ReadByHeadOfficeID,
        Clients_ReadComplex_ReadBySectionID,
        Clients_ReadComplex_ReadByEmail,
        Clients_ReadComplex_ClientsBankingInformation_ReadByCountryID,
        Clients_ReadComplex_ClientsShortNames_ReadByShortName,
        Clients_ReadComplex_ReadForCASSIdentification,
        SectionRoles_ReadComplex_ReadBySectionID,
        SectionRoles_ReadComplex_SectionRolesUsers_ReadByUserID,
        SectionRoles_ReadComplex_SectionRolesRights_ReadByField1,
        Periods_ReadComplex_ReadByPeriodCode,
        Periods_ReadComplex_ReadByLegacyPeriodCode,
        Workflow_ReadComplex_ReadByCode,
        Workflow_ReadComplex_ReadByCanUserCreate,
        WorkflowActivity_ReadComplex_ReadByCode,
        WorkflowActivity_ReadComplex_PreviousActivities_ReadByField1,
        WorkflowInstance_ReadComplex_ReadByParentWorkflowInstanceID,
        WorkflowActivityInstance_ReadComplex_ReadByWorkflowInstanceID,
        WorkflowActivityRights_ReadComplex_ReadByRoleID,
        WorkflowActivityRights_ReadComplex_ReadByWorkflowID,
        WorkflowActivityRights_ReadComplex_ReadByActivityID,
        UserSection_ReadComplex_ReadByUserID,
        CalendarEvent_ReadComplex_ReadByName,
        CalendarEvent_ReadComplex_ReadByStartDate,
        CalendarEvent_ReadComplex_ReadByPeriodID,
        CalendarEvent_ReadComplex_ReadByLinkedEventID,
        CalendarEvent_ReadComplex_ReadBySystemDateID,
        AgentInfoFiles_ReadComplex_ReadByFileName,
        AgentInfoFiles_ReadComplex_ReadByStatusID,
        AgentInfoFiles_ReadComplex_ReadByCreatedDate,
        AgentInfoFiles_ReadComplex_AgentInfoFilesImportLogs_ReadByField1,
        BillingFiles_ReadComplex_ReadByFileName,
        BillingFiles_ReadComplex_ReadByPeriodID,
        BillingFiles_ReadComplex_ReadBySectionID,
        BillingFiles_ReadComplex_ReadByWorkflowInstanceID,
        BillingFiles_ReadComplex_ReadByStatusID,
        BillingFiles_ReadComplex_ReadByFileFormatID,
        BillingFiles_ReadComplex_ReadByManuallyUploaded,
        BillingFiles_ReadComplex_BillingFilesTrans_ReadByInitiatorClient,
        BillingFiles_ReadComplex_BillingFilesTrans_ReadByReceiverClient,
        BillingFiles_ReadComplex_BillingFilesTrans_ReadByCreationDate,
        BillingFiles_ReadComplex_BillingFilesTrans_ReadByValueDate,
        BillingFiles_ReadComplex_BillingFilesTrans_ReadByTransactionDate,
        BillingFiles_ReadComplex_BillingFilesImportLogs_ReadByField1,
        BillingFiles_ReadComplex_BillingFilesAdjWarnings_ReadByPeriodID,
        BillingTransactions_ReadComplex_ReadByBillingFile,
        BillingTransactions_ReadComplex_ReadByInitiatorClient,
        BillingTransactions_ReadComplex_ReadByReceiverClient,
        BillingTransactions_ReadComplex_ReadByCreationDate,
        BillingTransactions_ReadComplex_ReadByValueDate,
        BillingTransactions_ReadComplex_ReadByTransactionDate,
        ImportationLogs_ReadComplex_ReadByErrorCode,
        BankMovements_ReadComplex_ReadByAccountOwner,
        UntraceableSwiftFiles_ReadComplex_ReadByFileTypeID,
        ActivityLog_ReadComplex_ReadByCategoryID,
        ActivityLog_ReadComplex_ReadBySeverityID,
        ActivityLog_ReadComplex_ReadByActivityDateTime,
        ActivityLog_ReadComplex_ReadByUserID,
        FieldChangeTracking_ReadComplex_ReadByTableID,
        FieldChangeTracking_ReadComplex_ReadByFieldID,
        PaymentInstructionFiles_ReadComplex_ReadByTypeID,
        PaymentInstructionFiles_ReadComplex_ReadByStatusID,
        PaymentInstructionFiles_ReadComplex_ReadByCreatedDate,
        PaymentInstructionFiles_ReadComplex_PaymentAssociatedFiles_ReadByFileCodeID,
        AdjustmentFiles_ReadComplex_ReadByTypeID,
        AdjustmentFiles_ReadComplex_ReadByStatusID,
        AdjustmentFiles_ReadComplex_AdjustmentFilesDetails_ReadByClient,
        ICCSReports_ReadComplex_ReadByOperationID,
        ICCSReports_ReadComplex_ReadByPeriodID,
        ICCSReports_ReadComplex_ReadByReportStatusID,
        ICCSReports_ReadComplex_ReadByCreatedDate,
        ClientBalances_ReadComplex_ReadAll,
        Clawbacks_ReadComplex_ReadAll,
        CompensationMngt_ReadComplex_ReadAll,

        SettlementAdjustments_ReadComplex_ReadAll,
        ManualPayments_ReadComplex_ReadAll,
        SalesForceInfos_ReadComplex_ReadAll,
        Settlement_ReadComplex_ReadAll,
        TopmanScreenShots_ReadComplex_ReadAll,
        ControlResults_ReadComplex_ReadAll,
        Matching_ReadComplex_ReadAll,
        ExpectedMovement_ReadComplex_ReadAll,
        Reports_ReadComplex_ReadAll,
        ShortNonPaymentMngt_ReadComplex_ReadAll,
        Countries_ReadComplex_ReadAll,
        Banks_ReadComplex_ReadAll,
        Currencies_ReadComplex_ReadAll,
        Sections_ReadComplex_ReadAll,
        HeadOffices_ReadComplex_ReadAll,
        Clients_ReadComplex_ReadAll,
        SectionRoles_ReadComplex_ReadAll,
        Notifications_ReadComplex_ReadAll,
        Periods_ReadComplex_ReadAll,
        Workflow_ReadComplex_ReadAll,
        WorkflowActivity_ReadComplex_ReadAll,
        WorkflowInstance_ReadComplex_ReadAll,
        WorkflowActivityInstance_ReadComplex_ReadAll,
        WorkflowActivityRights_ReadComplex_ReadAll,
        UserSection_ReadComplex_ReadAll,
        CalendarEvent_ReadComplex_ReadAll,
        AgentInfoFiles_ReadComplex_ReadAll,
        BankStatementFiles_ReadComplex_ReadAll,
        BillingFiles_ReadComplex_ReadAll,
        BillingTransactions_ReadComplex_ReadAll,
        ImportationLogs_ReadComplex_ReadAll,
        BankMovements_ReadComplex_ReadAll,
        UntraceableSwiftFiles_ReadComplex_ReadAll,
        ActivityLog_ReadComplex_ReadAll,
        FieldChangeTracking_ReadComplex_ReadAll,
        PaymentInstructionFiles_ReadComplex_ReadAll,
        AdjustmentFiles_ReadComplex_ReadAll,
        ICCSReports_ReadComplex_ReadAll,

        PickListValues_ReadComplex_GetPickListValues,
        PickListValues_ReadComplex_GetPickListValuesByParentCode,
        PickListValues_ReadComplex_GetMultiplePickListValues,
        PickListValues_ReadComplex_ReadPickListValues,

        //Custom ReadComplex operations go here...
        Sections_ReadComplex_ReadByUserID,
        Clients_ReadComplex_GetGroupCodesForClientBalance,
        Periods_ReadComplex_GetFromPeriodDT,
        Clients_ReadComplex_ReadAllClients,
        Clients_ReadComplex_ReadAllHeadOfficeCode,
        Sections_ReadComplex_ReadAllSectionName,

        BankStatements_ReadComplex_GetDateLastMigration,
        BankStatements_ReadComplex_GetBankStatements_Count,
        Clients_ReadComplex_GetClientExists,
        Periods_ReadComplex_GetAllFromPeriodUniqueDt,

        Clients_ReadComplex_GetClientBalanceDetailsForEditing,
        Periods_ReadComplex_GetOpenPeriodsBySectionID,
        Clients_ReadComplex_GetClientsFormatted,
        BankStatements_ReadComplex_GetBalances,
        Movements_ReadComplex_GetNetAmount,
        BankStatements_ReadComplex_GetStatements,
        Clients_ReadComplex_ReadAllAirlines,
        Movements_ReadComplex_ReadMovementsNetDetailsSMS,
        Sections_ReadComplex_GetSectionByUser,
        HeadOffices_ReadComplex_GetAHeadOfficeByCode,
        Sections_ReadComplex_GetAllSections,
        Periods_ReadComplex_GetPeriodsPaged,
        ActionInto_ReadComplex_ReadBySectionCode,
        Clients_ReadComplex_GetAirlineNames,
        ActionInto_ReadComplex_GetActInto,
        ClawbackActivate_ReadComplex_GetActivePlanDetails,
        Periods_ReadComplex_GetAllFromPeriodsUnique,
        Clients_ReadComplex_GetClients,
        Movements_ReadComplex_GetModifyPayment,
        Users_ReadComplex_GetUserByCode,
        Movements_ReadComplex_ReadMovementsHOFromToAgroupedPaged,
        Movements_ReadComplex_ReadAirlineNegativeBalancesPage,
        Movements_ReadComplex_ReadMovementsHOFromTo,
        Countries_ReadComplex_GetAllCountriesSorted,
        Currencies_ReadComplex_GetAllCurrency,
        Countries_ReadComplex_GetCountriesPaged,
        BankStatements_ReadComplex_ReadStatementAmountSectionPaged,
        Sections_ReadComplex_ReadCheckSectionBSP,
        Periods_ReadComplex_ReadPeriodRemittanceMoreClosing,
        Movements_ReadComplex_ReadCheckRegisterReferenceNotNullMovements,
        Sections_ReadComplex_ReadCheckSectionCASS,
        Clients_ReadComplex_GetAirlineCode,
        Clients_ReadComplex_GetAirlinesCode,
        AgencyDebitCredit_ReadComplex_GetBillingCode,
        AgencyDebitCredit_ReadComplex_GetClientCode,
        AgencyDebitCredit_ReadComplex_GetFileDate,
        AgencyDebitCredit_ReadComplex_GetADCMNumber,
        AgencyDebitCredit_ReadComplex_BuiltSumAmountViewSPDRSPCR,
        AgencyDebitCredit_ReadComplex_GetMonthSPDR_SPC,
        AgencyDebitCredit_ReadComplex_GetFileSPDR_SPCRNMovement,
        Periods_ReadComplex_GetPeriodSPDRSPCR,
        Periods_ReadComplex_GetDisctinctPeriodBySectionCode,
        Movements_ReadComplex_ReadMovementsHOPeriodGrouped,
        Movements_ReadComplex_ReadMovementsHOPeriod,
        AgencyDebitCredit_ReadComplex_ReadADCMHOFromTo,
        AgencyDebitCredit_ReadComplex_ReadADCMClient,
        AgencyDebitCredit_ReadComplex_ReadADCMAirlineSectionFromTo,
        BankStatements_ReadComplex_ReadStatementSectionFromToPaged,
        Clients_ReadComplex_GetCodeFromSection,
        Periods_ReadComplex_GetSettlementDaySA,
        Periods_ReadComplex_ReadPeriod,
        Periods_ReadComplex_ReadDates,
        Movements_ReadComplex_ReadSqlMovementsRefNull,
        SectionAgentCodes_ReadComplex_GetSectionAgentPaged,
        AgencyDebitCredit_ReadComplex_CalculateActiveClawBack,
        Clients_ReadComplex_ReadByHOCodeAndSectionCode,
        Adjustments_ReadComplex_ReadBySectionCodeAndSettlementDate,
        Movements_ReadComplex_ReadSqlMovementsHOCode,
        Movements_ReadComplex_ReadSqlMovementsClientCode,
        BankStatements_ReadComplex_ReadSqlStatements,
        Periods_ReadComplex_ReadSettlementDate,
        Clients_ReadComplex_ReadDataGridICCSReport,
        Periods_ReadComplex_ReadDataRptCabICCSReport,
        Clients_ReadComplex_ReadDataTotICCSReport,
        Periods_ReadComplex_ReadFromDateHAR,
        Movements_ReadComplex_ReadMovementsSelected,
        BankStatements_ReadComplex_ReadStatementsWithoutReference,
        AgencyDebitCredit_ReadComplex_GetAgencyDebitCreditPaged,
        Periods_ReadComplex_ReadAllPeriodsMonthAllSections,
        Periods_ReadComplex_CheckRemittanceDay,
        Clients_ReadComplex_ReadImpClienteAirlineDefRpt,
        Movements_ReadComplex_Count_GetMovementsByReference,
        HeadOffices_ReadComplex_GetAHeadOffice,
        Movements_ReadComplex_ReadCPCReportSuspended,
        Statements_ReadComplex_ReadCPCReportCreditLine,
        Clients_ReadComplex_ReadAirlineNameRptAirlineDef,
        Sections_ReadComplex_ReadNomFileAirlineDefRpt,
        Periods_ReadComplex_ReadAllPeriodsMonthbySection,
        Countries_ReadComplex_ReadCountryCodeBySection,
        HeadOffices_ReadComplex_GetAirlineLevel,
        Clients_ReadComplex_ReadDataGridAirlinesTotSetReport,
        ActionInto_ReadComplex_ReturnMaxActionIntoID,
        Adjustments_ReadComplex_ReadDataRptAirlineSetRtf,
        Adjustments_ReadComplex_ReadDataADCMSRptAirlineSetRtf,
        Adjustments_ReadComplex_ReadDatosRptAirlineSetRtf,
        AgencyDebitCredit_ReadComplex_AgenGetAllAgencyDebitCredit,
        Periods_ReadComplex_GetAllFromPeriod,
        BankStatements_ReadComplex_PaymentBreakdownUpdateStatement,
        Movements_ReadComplex_UpdateStatement,
        Movements_ReadComplex_UnmatchMovementsValidate,
        Nets_ReadComplex_ReadAirlineDefaultProrrata,
        Movements_ReadComplex_TotalMovements,
        Movements_ReadComplex_TotalAmountByIDs,
        Movements_ReadComplex_ReadByIDs,
        Movements_ReadComplex_ReadAfterClosingDate,
        Movements_ReadComplex_ReadConfirmDeleteNullMovements,
        Nets_ReadComplex_ReadMonthlyProrrateo,
        Movements_ReadComplex_ReadAirlineDefault,
        Movements_ReadComplex_ReadTotalAirlineDefault,
        Clients_ReadComplex_ReadDataGridAirlinesSetReport,
        Users_ReadComplex_UserExistsInRS,
        Nets_ReadComplex_ReadNetDetailsAPSPaged,
        Nets_ReadComplex_ReadNetDetailsHAPPaged,
        Nets_ReadComplex_ReadNetDetailsHOPeriodPaged,
        SalesForce_ReadComplex_GetDefaultDate,
        Nets_ReadComplex_ReadNetDetailsPerAHPPaged,
        Users_ReadComplex_ReadSectionExite,
        Periods_ReadComplex_GetStartAndCuttOffDateForRecentlyClosedPeriod,
        Periods_ReadComplex_GetRemittanceDay,
        Movements_ReadComplex_ReadInsertMovements,
        Nets_ReadComplex_GetImportClientNivelAirlineSetAir,
        Inbound_ReadComplex_GetImporationsWithStatus,
        Movements_ReadComplex_GetSalesForce,
        CalendarEvent_ReadComplex_FindEventsForCalendar,
        Workflow_ReadComplex_ReadBySectionPeriod,
        Workflow_ReadComplex_ReadWorkflowActivityInstances,
        WorkflowActivityInstance_ReadComplex_GetNotificationInstance,
        Users_ReadComplex_GetUsersWithRoleID,
        WorkflowActivity_ReadComplex_GetFirstActivityWithConfig,
        WorkflowActivity_ReadComplex_GetNextActivityWithConfig,
        WorkflowInstance_ReadComplex_ReadByWorkflowPeriodSectionID,
        Workflow_ReadComplex_ReadAvailableWorkflowsForUserCreation,
        Sections_ReadComplex_ReadNotificationsByWorkflowActivityID,
        Workflow_ReadComplex_ReadStageListByOperations,
        Sections_ReadComplex_GetSectionFromCountryID,
        Periods_ReadComplex_ReadByFDLDSectionCode,
        ImportationLogs_ReadComplex_CountError,
        WorkflowActivityInstance_ReadComplex_GetActivitiesToRun,
        Workflow_ReadComplex_ReadActivitiesByUser,

        Sections_ReadComplex_ReadRolesByUserID,

        BankStatementFiles_ReadComplex_BankStatementFilesBankMov_ReadByTypeID,
        BankStatementFiles_ReadComplex_BankStatementFilesBankMov_ReadBySTPIndicator,
        BankStatementFiles_ReadComplex_BankStatementFilesBankMov_ReadByAccountCode,

        BankStatementFiles_ReadComplex_BankStatementFilesBankMov_ReadByBookDate,



        Workflow_ReadComplex_ReadWorkflowsByOperation,
        Workflow_ReadComplex_ReadWorkflowsByHubs,

        BillingFiles_ReadComplex_ReadContentForID,

        FieldChangeTracking_ReadComplex_ReadFieldChangeTracking,


        SettlementCheckListReport_ReadComplex_AirlinesSettlement,
        SettlementCheckListReport_ReadComplex_AgentsSettlement,
        SettlementCheckListReport_ReadComplex_BillingSummary,
        SettlementCheckListReport_ReadComplex_SettlementReview,
        SettlementCheckListReport_ReadComplex_AirlinesAdjustment,
        SettlementCheckListReport_ReadComplex_AgentAdjustment,
        SettlementCheckListReport_ReadComplex_AirlinesICCSBreakdown,
        Settlement_ReadComplex_GetCheckListReport,
        Sections_ReadComplex_ReadSectionBankAccountBySAID,





        Periods_ReadForEntityRelationsBySectionFirstDay,
        PaymentInstructionFiles_ReadComplex_GetInformation,
        Countries_ReadComplex_CheckCountryNameISOCUnicity,
        Countries_ReadComplex_CheckCountryNameBeforeDelete,

        Banks_ReadComplex_CheckNameTypeIDUnicity,
        Currencies_ReadComplex_CheckCurrencyNameUnicity,
        Notifications_ReadComplex_CheckNotificationsName,

        SectionRoles_ReadComplex_CheckRoleNameUnicity,
        SectionRoles_ReadComplex_CheckUserIdPerRoleIDUnicity,
        WorkflowActivityRights_ReadComplex_CheckActivityRolePersmissionUnicity,
        HeadOffices_ReadComplex_CheckHOCodeUnicity,
        Clients_ReadComplex_CheckClientCodeUnicity,
        Sections_ReadComplex_CheckSectionCodeUnicity,
        Sections_ReadComplex_CheckMatchOnUnicity,
        ImporationLogs_ReadComplex_HasImportationLogs,
        ClientBalances_ReadComplex_ReadByClientAndPeriodID,
        PaymentInstructionFiles_ReadComplex_ReadByFileName,
        BillingFiles_ReadComplex_ReadBillingInfoByWorkflowInstanceID,
        WorkflowActivityInstance_ReadComplex_ReadPrerequisite,
        WorkflowActivityInstance_ReadComplex_GetIDByBillingFilesID,
        WorkflowActivityInstance_ReadComplex_GetIDByAdjustmentFilesID,
        ClientBalances_ReadComplex_ReadForCreation,
        BankMovements_ReadComplex_GetUnmatchedMovements,

        BankStatementFiles_ReadComplex_ReadSectionBySAID,
        BankStatementFiles_ReadComplex_ReadByBankStatementInfoByWorkflowInstanceID,
        Periods_ReadComplex_GetPeriodsForMatching,
        WorkflowActivityInstance_ReadComplex_ReadWorkflowByID,
        WorkflowInstance_ReadComplex_ReadInstanceCompletedByPeriodID,
        WorkflowInstance_ReadComplex_ReadInstanceActiveByPeriodID,
        WorkflowInstance_ReadComplex_ReadInstanceActiveBySectionID,
        CalendarEvent_ReadComplex_GetWorkflowsToStart,
        CalendarEvent_ReadComplex_GetNotificationsToSend,
        CalendarEventImport_ReadComplex_ReadAll,
        Periods_ReadComplex_GetIDByPeriodCodeSectionID,
        Workflow_ReadComplex_ReadByName,
        BankStatementFiles_ReadComplex_ReadOriginalFileContent,
        BankStatementFiles_ReadComplex_ReadXMLFileContent,
        CalendarEvent_ReadComplex_ReadEventByPeriodIDLinkedEventID,
        Periods_ReadComplex_ReadByLDSectionCode,
        Periods_ReadComplex_ReadByRDLDSectionCode,
        Periods_ReadComplex_CheckPeriodCodeUnicity,
        SectionRoles_ReadComplex_CheckUserIdRoleIDPerOperationUnicity,
        Workflow_ReadComplex_ReadPeriodLevelByName,
        WorkflowInstance_ReadComplex_ReadInstanceActiveForHub,
        AgentInfoFiles_ReadComplex_ReadAgentFilesInfoByWorkflowInstanceID,
        PaymentInstructionFiles_ReadComplex_GetFileContent,
        PaymentInstructionFiles_ReadComplex_GetMT101XMLContent,
        Workflow_ReadForEntityRelationsByDashboardDisplayID,
        Sections_ReadForEntityRelationsPerUser,
        Sections_ReadForEntityRelationsPerUserPerClientType,

        PaymentInstructionFiles_ReadComplex_ReadByReferenceFilename,
        BanksMessageConfiguration_ReadComplex_CheckMessageTypeIDBankIDUnicity,

        HeadOffices_ReadForEntityRelationsBySection,
        Clients_ReadForEntityRelationsBySection,
        Clients_ReadForEntityRelationsByPeriod,

        AdjustmentFiles_ReadComplex_ReadForSending,
        AdjustmentFiles_ReadComplex_ReadFileContent,
        AgentInfoFiles_ReadComplex_ReadContentForID,
        Sections_ReadComplex_ReadSectionBankAccountByPaymentAccountNumber,
        Sections_ReadComplex_ReadSectionBankAccountByStatementAccountNumber,
        BankStatementFiles_ReadComplex_ReadSectionByStatementAccountNumber,
        Settlement_ReadComplex_HasSettlementDetails_ReadByWorkflowID,
        PaymentInstructionFiles_ReadComplex_ReadByWorkflowInstanceID,
        Periods_ReadForEntityRelationsBySection,
        Settlement_ReadComplex_ReadDetailsBreakDown,
        CalendarEvents_ReadComplex_ReadBySystemDateIDPeriodID,
        BillingFiles_ReadComplex_ReadAllForICCSReportByBillingFileID,
        BillingFiles_ReadComplex_ReadAllForICCSReportHeaderByBillingFileID,
        CalendarEvent_ReadComplex_ReadBySystemDateIDPeriodIDSettlementType,
        ICCSReports_ReadComplex_ByWorkflowInstanceID,
        ICCSReports_ReadComplex_FileContentByID,

        PaymentInstructionFiles_ReadComplex_GetFileIDByReferenceNumber,
        Settlement_ReadComplex_ReadAllForICCSReportBySettlementID,
        SettlementHarReport_ReadComplex_General,
        SettlementHarReport_ReadComplex_Agents,
        SettlementHarReport_ReadComplex_Airlines,
        SettlementHarReport_ReadComplex_Suspense,
        SettlementHarReport_ReadComplex_Details_Agents,
        SettlementHarReport_ReadComplex_Details_Airlines,
        SettlementHarReport_ReadComplex_Details_Suspense,

        BankStatementFiles_ReadComplex_ReadByStatementNumberAccountNumber,
        BankStatementFiles_ReadComplex_CountFilePartsForStatement,
        BankStatementFiles_ReadComplex_ReadFirstSequence,
        BankStatementFiles_ReadComplex_ReadLastSequence,
        BankStatementFiles_ReadComplex_ReadClosingAvailableSequence,
        BankStatementFiles_ReadComplex_ReadBankStatementPartParsedContent,

        UntraceableSwiftFiles_ReadComplex_GetFilestreamContent,

        Settlement_ReadComplex_GetHarReport,
        ManualPayments_ReadComplex_HasManualPaymentDetails,
        Banks_ReadComplex_ReadBankMessageConfigByMessageType,
        Settlement_ReadComplex_ReadByHarWorkflowInstanceID,
        Clients_ReadComplex_ReadClientSnapshotByPeriod,
        Sections_ReadComplex_ReadHubSection,
        ManualPayments_ReadComplex_ReadByWorkflowInstanceID,
        ManualPayments_ReadComplex_GetInformation,
        ICCSReports_ReadComplex_ICCSSend,
        WorkflowActivityInstance_ReadComplex_ReadActiveActivityByWorkflowCode,
        ClientBalances_ReadComplex_ForEntityRelationsByPeriod,
        WorkflowActivityInstance_ReadComplex_ReadNonCanceledByWorkflowCodePeriodID,
        WorkflowActivityInstance_ReadComplex_ReadNonCanceledByActivityIDPeriodID,
        WorkflowActivityInstance_ReadComplex_ActivityIsNotCanceledOrError,
        SettlementHar_ReadComplex_GetHarReportByWorkflowActivityInstanceId,
        Clients_ReadForEntityRelationsICCS,
        Sections_ReadComplex_ICCSConnectionInfoFromHub,
        WorkflowActivityInstance_ReadComplex_ReadDataForMailMerge,
        CalendarEvent_ReadComplex_ReadDataForMailMerge,
        //ManualMatching_ReadComplex_ReadManualMatchingImport,
        ClawbacksMngt_ReadComplex_ReadImportData,
        BankStatementFiles_ReadComplex_ReadBankStatementPartBySequenceNumber,
        ImportationLogs_ReadComplex_CountWarning,

        ICCSReports_ReadComplex_ReadBillingInfoByWorkflowInstanceID,
        Periods_ReadComplex_CanDeletePeriod,
        Matching_ReadComplex_CountTimeStamp,
        Matching_ReadComplex_GetConfirmedMatchingByWorkflowInstanceID,
        Matching_ReadComplex_GetConfirmedMatchingByBankMovementID,
        Sections_ReadComplex_BSPConnectionInfoFromHub,
        Matching_ReadComplex_ReadForCreation,
        Sections_ReadComplex_ReadControlActivationBySectionID,
        Sections_ReadComplex_ReadBankAccountsUsedForPaymentsBySectionID,
        Sections_ReadComplex_ReadMostRecentUsedBankAccountsBySectionID,
        Settlement_ReadComplex_ReadSettlementDetailsByID,
        ManualPayments_ReadComplex_ReadManualPaymentDetailsByID,
        ProrationToClawback_ReadComplex_ReadByClawbackID,
        ProrationToClawback_ReadComplex_ReadByProrationPlanID,
        HeadOffices_ReadComplex_ReadAirlinesBySectionID,
        Clients_ForEntityRelation_ReadByHeadOfficeAndPeriod,
        ClientBalances_ReadComplex_NegRemittanceBySectionPeriod,
        Prorations_ReadComplex_ReadByPeriodID,
        Prorations_ReadComplex_ReadByWorkflowInstanceID,
        HeadOffices_ReadComplex_ReadForEntityRelationsBySectionByPeriod,
        ICCSReports_ReadComplex_ReadSettlementByWorkflowInstanceID,
        AdjustmentFiles_ReadComplex_ByWorkflowInstanceID,
        Matching_ReadComplex_GetFindResultsWithoutPaging,
        Settlement_ReadComplex_ValidateSettlementSectionBankingInfo,

        ImportationLogs_ReadComplex_ReadByID,
        WorkflowActivityInstance_ReadComplex_ReadByWorkflowInstanceIDAndActivityCode,
        WorkflowActivityRights_ReadComplex_ReadPermissionTypeByActivityID,
        WorkflowActivityRights_ReadComplex_ReadByUsername,

        BankStatementFiles_ReadComplex_CheckFileNameAndStatus,
        BankStatementFiles_ReadComplex_CheckStatementNumberAndStatus,
        Sections_ReadComplex_BankingConfigBySectionId,
        ShortNonPaymentMngt_ReadComplex_ReadAllForExtraction,

        Clients_ReadComplex_ReadByClientCodeAndCurrencyCode,
        SalesForceSIDRACases_ReadComplex_ReadByShortNonPaymentID,
        SalesForceSIDRACases_ReadComplex_ReadAll,
        SalesForceSIDRACases_ReadComplex_GetAllByWorkflowInstanceIDForSending,
        ProrationPlans_ReadComplex_GetWorkflowProrationPlans,
        ProrationPlans_ReadComplex_GetByPeriodID,

        HAR_ReadComplex_ReadLastClosingBalance,
        HAR_ReadComplex_ReadStartDateUponCreation,
        HAR_ReadComplex_ReadCutOffDateUponCreation,
        HAR_ReadComplex_ReadByWorkflowInstanceID,
        HAR_ReadComplex_HasHarOnSameDay,

        Sections_ReadComplex_ReadAllAutomatchingRulesBySectionIDWorkflowID,
        SalesForceSIDRACases_ReadComplex_GetAllForSalesForceUpdate,
        WorkflowInstance_ReadComplex_ReadByWorkflowCodePeriodIDSectionID,

        CompensationMngt_ReadComplex_ReadByPeriodID,
        CompensationMngt_ReadComplex_ReadByDate,
        CompensationMngt_ReadComplex_ReadByOperationID,
        ExpectedMovement_ReadComplex_ReadByMatchingID,
        SettlementAdjustments_ReadComplex_ReadByClawbackID,
        Sections_ReadComplex_CASSConnectionInfoFromHub,
        Clients_ReadForEntityRelationsMasterByPeriod,
        Matching_ReadComplex_CountMatchesOnBankMovementByMatchID,
        Matching_ReadComplex_GetMatchesOfBankMovementByMatchID,

        BankMovements_ReadComplex_ReadMatchingsByStatus,
        Clients_ReadComplex_ReadClientsForEntityRelationsMasterBySection,
        Clients_ReadComplex_ReadAgentsAirlineForEntityRelationsMasterBySection,
        Clients_ReadComplex_ReadAirlinesForEntityRelationsMasterBySection,

        Sections_ReadComplex_ReadBySectionID,
        ClientBalances_ReadComplex_GetAllModifiedByOperation,
        HeadOffices_ReadForEntityRelationsBySectionForAgent,

        PickListValues_ReadComplex_ReadForEntityRelations,
        PickLists_ReadComplex_ReadForEntityRelations,
        PickListLanguages_ReadComplex_ReadForEntityRelations,
        PickListValues_ReadComplex_GetPickListValuesByPickListCode,
        PickListValues_ReadComplex_GetMultiplePickListValuesByPickListCodes,
        PickListValues_ReadComplex_PickListValueDescriptions_ReadByValueCode,
        PickListValues_ReadComplex_PickListValueDescriptions_CheckLanguageCodeUnicity,
        BankStatementFiles_ReadComplex_IsStatementRequiresControl,

        SectionRoles_ReadComplex_GetSectionRolesAssociation,
        WorkflowActivityInstance_ReadComplex_Signatory1CompletedByUser,
        ManualPayments_Report_ClientMissingBankingInformation,
        Settlement_Report_ClientsMissingBankingInformation,
        WorkflowActivityInstance_ReadComplex_GetIDByBankStatementFileID,
        BankStatementFiles_ReadComplex_ReadByWorkflowInstanceID,
        WorkflowActivityRights_ReadComplex_IsNotificationOnly,
        Sections_ReadComplex_SIDRAConnectionInfoFromHub,
        CompensationMngt_ReadComplex_ReadByGroupID,
        SettlementHar_ReadComplex_GetBankMovementsUnmatchedWithinHAR,
        Settlement_ReadForEntityRelationsByOperation,
        ManualPayments_ReadForEntityRelationsByOperation,
        Settlement_ReadComplex_NeedsICCSClient,
        AgentInfoFiles_ReadComplex_GetErrors,
        HAR_ReadComplex_GetCPCReport,
        Statements_Report_CPCCaculateMinimumReport,
        Statements_Report_CPCCreditLimitReport,
        Movements_Report_CPCSuspensionAirlineMovementsReport,
        Period_Report_GetPeriodList,
        Clients_ReadComplex_GetIDByAgentCodeSectionID,
        Clients_ReadComplex_GetShortNameID,
        SalesForceSIDRACases_ReadComplex_GetAllByWorkflowInstanceIDSent,
        Clients_ReadComplex_GetReferencesCountByClientID,
        HeadOffices_ReadComplex_GetReferencesCountByHeadOfficeID,
        WorkflowActivityInstance_ReadComplex_ActivityIsNotCanceled,
        WorkflowActivityInstance_ReadComplex_ReadCompletedWorkflowActivityByCodeByWorkflowInstanceID,
        Sections_ReadComplex_BankingConnectivityInfoFromHub,
        HAR_ReadComplex_ReadCurrentClosingBalance,
        Sections_ReadComplex_GetReferencesCountBySectionID,
        HeadOffices_ReadComplex_NotLinkedToClientWithSameCode,
        HAR_ReadComplex_AgentsPostTerminationAccountMovements,
        Workflow_ReadForEntityRelationsByRole,
        HeadOffices_ReadComplex_BySubClientTypeBySection,
        SalesForceSIDRACases_ReadComplex_ReadAllSuccessfullySentByPeriod,
        AdjustmentFiles_ReadComplex_GenerateIATAClawbacks,

        Currencies_ReadComplex_ReadSectionLinkedWithCurrency,
        SettlementHar_ReadComplex_ReadTopmanScreensIncluded,
        Prorations_ReadComplex_GetProrationSums,
        Periods_ReadComplex_GetCurrentPeriod,
        WorkflowActivityInstance_ReadComplex_ReadActiveActivityByWorkflowCodePeriodID,

        Periods_ReadComplex_GetPeriodState,
        Settlement_ReadComplex_GetBSPLinkSummaryPackage,
        Settlement_ReadComplex_GetBSPLinkSummaryReport,
        Settlement_ReadComplex_GetBSPLinkSettlementPackage,
        Settlement_ReadComplex_GetBSPLinkSettlementReport,
        HeadOffices_ReadComplex_ReadByHOCodeAndSectionCode,

        Countries_ReadComplex_GetIDByCountryName,
        ClawbacksMngt_ReadComplex_ReadByAdjustmentFileID,

        SalesForceSIDRACases_ReadComplex_ReadSidraCasesForUpdateForClosedPeriod,
        ControlResults_ReadComplex_AvoidCollision,
        ManualMatching_ReadComplex_ReadIncorrectClientCodes,
        ManualMatching_ReadComplex_ReadIncorrectPeriodCodes,
        ManualMatching_ReadComplex_ImportValidateDecimalPlacement,
        Periods_ReadComplex_GetHARDayOfLastOngoingPeriodForSection,
        Periods_ReadComplex_GetPaymentAndAdjustmentFileIDs,
        Sections_ReadComplex_GetClosedPeriodIDsForSection,
        Sections_ReadComplex_GetPaymentAndAdjustmentSentAckCount,
        HAR_ReadComplex_GetClosingBalance,
        Sections_ReadComplex_CanCreateAgentCode,
        WorkflowActivityInstance_ReadComplex_ActivityIsNotRunningByWorkflow,

        ExternalMovementMngt_ReadComplex_ReadByOperationID,
        ExternalMovementMngt_ReadComplex_ReadByPeriodID,
        ExternalMovementMngt_ReadComplex_ReadByClient,
        ExternalMovementMngt_ReadComplex_ReadByAirline,
        ExternalMovementMngt_ReadComplex_ReadByExtMovementDate,
        ExternalMovementMngt_ReadComplex_ReadByDefaultDate,
        ExternalMovementMngt_ReadComplex_ReadBySIDRACaseNumber,
        ExternalMovementMngt_ReadForEntityRelations,
        ExternalMovementMngt_ReadComplex_ReadAll,
        Sections_ReadComplex_ReadByPeriodID,
        Clawbacks_ReadComplex_ReadByExternalMovementID,
        CompensationMngt_ReadComplex_ReadByExternalMovementID,
        Currencies_ReadComplex_GetCurrencyDataBySectionCode,
        Currencies_ReadComplex_GetCurrencyDataByClientID,
        IBANStructure_ReadComplex_ReadByIBANCountryCode,
        IBANStructure_ReadComplex_ReadAll,
        BankDirectoryPlus_ReadComplex_ReadByRecordKey,
        BankDirectoryPlus_ReadComplex_ReadAll,
        MessageStandardFields_ReadForEntityRelations,
        MessageStandardFields_ReadComplex_GetAccountFieldsByEntityTypeID,
        Sections_ReadComplex_SectionPain001BR_ReadByThresholdValue,
        Sections_ReadComplex_SectionPain008BR_ReadByThresholdValue,
        Sections_ReadComplex_SectionMT101BR_ReadByThresholdValue,
        Sections_ReadComplex_SectionMT104BR_ReadByThresholdValue,
        AccountInfo_ReadComplex_ReadByBeneficiaryAccountID,
        AccountInfo_ReadComplex_ReadAll,
        AccountInfo_ReadComplex_GetHingeAccountFields,
        Sections_ReadComplex_QueryAllOutgoingConnectionStatus,

        HAR_ReadComplex_HARSupportingDocuments_ReadByIDHAR,
        HAR_ReadComplex_GetHARSupportingDocumentFile,

        HAR_ReadComplex_GetHARWithHARZipFile,

        HAR_ReadComplex_ReadHARWithWORKFLOWSTATUS,

        HAR_ReadComplex_CountApprovedHARInPeriod,

        Settlement_ReadComplex_SettlementSignatoryCheckUserRole,
        Settlement_ReadComplex_SettlementSignatoryCheckUserUnicity,
        Settlement_ReadComplex_SettlementSignatory_GetSignatoriesQuantity,

        WorkflowActivityInstance_ReadComplex_SettlementSignatoriesActivityControl,
        Settlement_ReadComplex_SettlementSignatoriesByWorkFlowActivityInstaceID,

        Settlement_ReadComplex_SettlementSignatoryCheckActualUserRole,

        ManualPayments_ReadComplex_ManualPaymentsSignatoryCheckUserRole,
        ManualPayments_ReadComplex_ManualPaymentsSignatoryCheckUserUnicity,
        ManualPayments_ReadComplex_ManualPaymentsSignatories_GetSignatoriesQuantity,

        WorkflowActivityInstance_ReadComplex_ManualPaymentsSignatoriesActivityControl,
        ManualPayments_ReadComplex_ManualPaymentsSignatoriesByWorkFlowActivityInstaceID,

        ManualPayments_ReadComplex_ManualPaymentsSignatoryCheckActualUserRole,

        //Discrepncy
        RemDescReports_ReadComplex_ReadBySectionID,
        RemDescReports_ReadComplex_ReadByName,
        RemDescReports_ReadComplex_ReadByTypeID,
        RemDescReports_ReadComplex_ReadByStatusID,
        RemDescReports_ReadComplex_ReadAll,
        RemDescReports_ReadComplex_GetReportFile,
        RemDescReports_ReadComplex_ByWorkflowInstanceID,
        Sections_ReadComplex_RemDescReportsConnectionInfoFromHub,
        RemDescReports_ReadComplex_RemDescReportsSend,

        BillingAdjustments_ReadComplex_ReadByOperationID,
        BillingAdjustments_ReadComplex_ReadByPeriodID,
        BillingAdjustments_ReadComplex_ReadByAgentID,
        BillingAdjustments_ReadComplex_ReadByAirlineID,
        BillingAdjustments_ReadComplex_ReadByApprovalStatusID,
        BillingAdjustments_ReadComplex_ReadAll,
    }

    /// <summary>
    /// An enumeration of the unique ExecuteComplex identifiers.
    /// The SaveFileContent and SaveDocument operations have identifiers,
    /// as well as any other custom operation that needs to be added.
    /// </summary>
    public enum ExecuteComplexID
    {
        //All new ones
        TopmanScreenShots_ExecuteComplex_SaveContentFileContent,
        TopmanScreenShots_ExecuteComplex_SaveDocument,
        HeadOffices_ExecuteComplex_DoAddition,
        Movements_ExecuteComplex_BatchInsertMovements,
        Clients_ExecuteComplex_DeleteClient,
        HeadOffices_ExecuteComplex_DeleteHeadOffice,
        Periods_ExecuteComplex_DeletePeriod,
        HeadOffices_ExecuteComplex_GetClientTypeIsAgent,
        Users_ExecuteComplex_DeleteUser,
        SalesForceInfos_ExecuteComplex_GetLastImportDate,
        Periods_ExecuteComplex_CheckExistsPeriod,
        ClawbackActivate_ExecuteComplex_GetCutOffDate,
        ActionInto_ExecuteComplex_GetCutOffDate,
        Sections_ExecuteComplex_GetClosingDate,
        Sections_ExecuteComplex_GetNextFileSequenceNumber,
        Periods_ExecuteComplex_GetDatePeriod,
        HeadOffices_ExecuteComplex_GetClientType,
        Movements_ExecuteComplex_GetSumAmount,
        ActionInto_ExecuteComplex_AmountRecovered,
        Movements_ExecuteComplex_ModifyStatus,
        Inbound_ExecuteComplex_ImportAIMSFile,
        Inbound_ExecuteComplex_ImportBSPFile,
        CalendarEvent_SaveFromSchedule,
        CalendarEvent_DeleteFromSchedule,
        AgentInfoFiles_ExecuteComplex_ImportFile,
        BillingFiles_ExecuteComplex_ImportFile,
        BillingFiles_ExecuteComplex_ValidateContent,
        BillingFiles_ExecuteComplex_ContentToTrans,
        BillingFiles_ExecuteComplex_ManuallyImportFile,
        BankStatement_ExecuteComplex_ImportFile,
        BankStatement_ExecuteComplex_ManuallyImportFile,
        Settlement_ExecuteComplex_CreateSettlement,
        Settlement_ExecuteComplex_GenerateSettlementCheckListReport,
        PaymentInstructionFiles_ExecuteComplex_ImportFile,
        PaymentInstructionFiles_ExecuteComplex_UpdateFileStatus,
        PaymentInstructionFiles_ExecuteComplex_UpdatePaymentInstructionStatus,
        PaymentInstructionFiles_ExecuteComplex_UpdateSettlementDetailTransactionNumber,
        Matching_ExecuteComplex_UpdateMatchStatus,
        Matching_ExecuteComplex_SaveMatching,
        Matching_ExecuteComplex_MarkDispute,
        Matching_ExecuteComplex_ImportMatch,

        ActivityLog_ExecuteComplex_LogBankingEvent,
        PaymentInstructionFiles_ExecuteComplex_SendFileToBizTalk,

        ExpectedMovement_ExecuteComplex_CreateFromBillingFiles,
        ClientBalances_ExecuteComplex_UpdateAmounts,
        CalendarEvent_ExecuteComplex_ValidatePeriod,

        Matching_ExecuteComplex_MatchLocationNet,
        Matching_ExecuteComplex_MatchGroupNet,
        Matching_ExecuteComplex_MatchIATACode,
        Matching_ExecuteComplex_MatchShortName,
        Matching_ExecuteComplex_ManualMatch,
        Matching_ExecuteComplex_MatchLocationPending,
        Matching_ExecuteComplex_MatchGroupPending,
        Matching_ExecuteComplex_MatchLocationAdjustmentNet,

        Section_ExecuteComplex_ReadIncrementMT101Sequence,
        Section_ExecuteComplex_ReadIncrementMT101FileSequence,

        BillingFiles_ExecuteComplex_AcceptBilling,
        BillingFiles_ExecuteComplex_RejectBilling,
        BillingFiles_ExecuteComplex_UpdateWorkflowInstanceID,
        Matching_ExecuteComplex_UndoMatching,
        BankStatementFiles_ExecuteComplex_UpdateWorkflowInstanceID,
        Notifications_ExecuteComplex_SendEmail,
        CalendarEvent_ExecuteComplex_UploadEvents,
        CalendarEventImport_ExecuteComplex_DeleteAll,
        CalendarEvent_ExecuteComplex_SetLinkedEventCreatedToTrue,
        WorkflowInstance_ExecuteComplex_AssignOwnership,
        Settlement_ExecuteComplex_GenerateSettlementDetails,

        AdjustmentFiles_ExecuteComplex_GetBSPClawbacksData,
        AdjustmentFiles_ExecuteComplex_GetCASSClawbacksData,
        AdjustmentFiles_ExecuteComplex_Generate,
        AdjustmentFiles_ExecuteComplex_SendFiles,

        CassBillingFiles_ExecuteComplex_ImportFile,
        BillingFiles_ExecuteComplex_CASSValidateContent,
        AgentInfoFiles_ExecuteComplex_UpdateWorkflowInstanceID,
        AgentInfoFiles_ExecuteComplex_Validate,
        AgentInfoFiles_ExecuteComplex_Import,
        Settlement_ExecuteComplex_UpdateStatus,
        Settlement_ExecuteComplex_DeleteSettlement,
        Settlement_ExecuteComplex_DeleteSettlementDetails,


        ActivityLog_ExecuteComplex_LogPaymentInstructionFileEvent,
        BillingFiles_ExecuteComplex_CASSContentToTrans,

        Clawbacks_ExecuteComplex_UpdateClawbacksToUnconfirmed,
        Clawbacks_ExecuteComplex_CancelAdjustmentFilesSending,
        Clawbacks_ExecuteComplex_ValidateBusinessRules,
        Settlement_ExecuteComplex_UpdateWorkflowInstanceID,

        PaymentFiles_ExecuteComplex_SaveAssociatedFiles,
        SettlementHar_ExecuteComplex_GenerateHarReport,
        BankStatementFiles_ExecuteComplex_UpdateFilePartContent,
        HeadOffices_ExecuteComplex_MasterDataSnapshot_Save,
        BankStatementFiles_ExecuteComplex_DeleteAllMovementsByStatement,
        CalendarEvent_ExecuteComplex_DeleteAllByPeriod,
        ShortNonPaymentMngt_ExecuteComplex_ExtractByOperationIDPeriodID,
        ShortNonPaymentMngt_ExecuteComplexTask_SwitchInclusionStatus,
        BillingFiles_ExecuteComplex_ValidateClientsWithSnapshot,
        Settlement_ExecuteComplex_CreateHarWorkflow,
        SettlementHar_ExecuteComplex_UpdateHarReport,
        SettlementHar_ExecuteComplex_UpdateHarReportWithTopmanScreens,
        ManualPayments_ExecuteComplex_UpdateStatus,
        ManualPayments_ExecuteComplex_UpdateManualPaymentDetailTransactionNumber,
        ManualPayments_ExecuteComplex_ImportPaymentDetails,
        Clients_ExecuteComplex_UpdateBankingInformationStatus,
        HeadOffices_ExecuteComplex_MasterDataSnapshot_DeleteByPeriodID,
        ClientBalances_ExecuteComplex_UpdateOnMasterDataSnapshot,
        CassBillingFiles_ExecuteComplex_ExportFile,

        ManualMatching_ExecuteComplex_ImportManualMatchingFile,
        ManualMatching_ExecuteComplex_ManualImportUpdateIDS,
        ManualMatching_ExecuteComplex_DeleteManualMatchingData,
        ClawbacksMngt_ExecuteComplex_ImportClawbacksFile,
        ClawbacksMngt_ExecuteComplex_ImportClawbacksUpdateIDs,
        ClawbacksMngt_ExecuteComplex_DeleteImportData,
        Matching_ExecuteComplex_SaveClientPeriod,


        CalendarEvent_ExecuteComplex_DeleteFurtureEventsForPeriod,
        Settlement_ExecuteComplex_UpdateSettlementDetailsInclude,
        Matching_ExecuteComplex_DeleteAllFromSearch,
        Sections_ExecuteComplex_UpdateControlActivationIsActive,
        Matching_ExecuteComplex_MatchPayments,
        Sections_ExecuteComplex_SetSettlementType,

        Settlement_ExecuteComplex_SaveHingeSnapshot,
        Prorations_ExecuteComplex_CreateAgentProrations,
        Prorations_ExecuteComplex_CreateAgentProrationPlans,
        Prorations_ExecuteComplex_CreateAirlineProrationPlans,
        ClientBalances_ExecuteComplex_UpdateApprovalStatus,
        ICCS_ExecuteComplex_UpdateSendStatus,
        AdjustmentFiles_ExecuteComplex_UpdateSendStatus,
        BillingFiles_ExecuteComplex_CASSValidateClientsWithSnapshot,
        Sections_ExecuteComplex_InsertSectionControlActivationsForSection,
        Sections_ExecuteComplex_CloneAssociations,
        Prorations_ExecuteComplex_CalculateAgentsMethod,
        Prorations_ExecuteComplex_CalculateAirlineDefaultMethod,
        Prorations_ExecuteComplex_CreateAirlineProrations,
        //Prorations_ExecuteComplex_CalculateDeductionsAccumulated,
        //Prorations_ExecuteComplex_CalculateDeductionsProgressive,
        Prorations_ExecuteComplex_CalculateDeductions,
        BillingFiles_ExecuteComplex_ClearBillingFileContentForBillingFileID,
        ClientBalances_ExecuteComplex_SwitchInclusionStatusByGroupByPeriod,
        ClientBalances_ExecuteComplex_SetShortNonPaymentDatesByOperationByPeriod,
        Prorations_ExecuteComplex_DeleteProration,
        Prorations_ExecuteComplex_DeleteProrationPlans,
        ShortNonPaymentMngt_ExecuteComplex_ClearShortNonPaymentsForOperationPeriod,
        SalesForceSIDRACases_ExecuteComplex_CreateFromShortNonPayments,
        SalesForceSIDRACases_ExecuteComplex_DeleteAllByWorkflowInstanceID,
        Sections_ExecuteComplex_CreateAutoRulesSnapshot,
        Sections_ExecuteComplex_ClearAutoRulesSnapshot,
        Matching_ExecuteComplex_ClearMatchesByWorkflowInstanceByStatus,
        Matching_ExecuteComplex_Rematch,
        SalesForceSIDRACases_ExecuteComplex_UpdateCase,
        Sections_ExecuteComplex_ReloadWorkflowsAutomatchingRules,
        Sections_ExecuteComplex_OverWriteSectionAutomatchingRulesWithWorkflows,
        BillingFiles_ExecuteComplex_GenerateTransactionWarnings,

        ClientBalances_ExecuteComplex_UpdatePreviousBalances,
        BillingFiles_ExecuteComplex_UndoSanityCheck,
        SalesForceSIDRACases_ExecuteComplex_UpdateFromShortNonPayments,
        Periods_ExecuteComplex_ClosePeriod,

        HAR_ExecuteComplex_UpdateStatusIDForWorkflowInstanceActivityID,
        Settlement_ExecuteComplex_DeleteSettlementAdjustments,
        Matching_ExecuteComplex_DeleteAllMatchesOfBankMovementByMatchID,
        ClientBalances_ExecuteComplex_CreateMissing,
        Clients_ExecuteComplex_AddShortNameToClient,
        BillingFiles_ExecuteComplex_GenerateAdjustmentReports,
        BankStatementFiles_ExecuteComplex_SanityCheck_OpeningClosingBalances,
        BankStatementFiles_ExecuteComplex_SanityCheck_InsertBankMovements,
        PaymentInstructionFiles_ExecuteComplex_ImportRecoveredFile,
        Compensation_ExecuteComplex_DeleteAllUnconfirmedByGroupID,
        CompensationMngt_ExecuteComplex_UnconfirmByGroupID,
        CompensationMngt_ExecuteComplex_ConfirmByGroupID,
        CompensationMgt_ExecuteComplex_ImportCompensationFile,
        ClientBalances_ExecuteComplex_UpdateOnMasterDataSnapshotUndo,
        BillingFiles_ExecuteComplex_ConfirmClawbacksFromAdjustmentsReport,
        Settlement_ExecuteComplex_MatchSettlementDetailsWithRecoveredPaymentInstructions,
        Sections_ExecuteComplex_UpdateHingeBalance,
        ManualPayment_ExecuteComplex_SaveHingeSnapshot,
        Clawbacks_ExecuteComplex_ConfirmPostClawbacks,
        Sections_ExecuteComplex_CleanUp,
        Sections_ExecuteComplex_UpdateHubLiveMT101SequenceNumber,
        Periods_ExecuteComplex_CleanUp,
        SettlementAdjustments_ExecuteComplex_ImportSettlementAdjustmentsUpdateIDs,
        SettlementAdjustments_ExecuteComplex_CreateSettlementAdjustmentsFromImport,
        SettlementAdjustments_ExecuteComplex_DeleteImportData,
        SettlementAdjustments_ExecuteComplex_ImportSettlementAdjustmentsFile,
        BillingFiles_ExecuteComplex_UnconfirmAdjustmentReports,
        BankStatementFiles_ExecuteComplex_SetStatus,
        Clients_ExecuteComplex_ImportShortNames,
        AgentInfoFiles_ExecuteComplex_ImportFileManually,
        PaymentInstructionFiles_ExecuteComplex_DeleteAssociatedToSettlementByWorkflowInstanceID,
        PaymentInstructionFiles_ExecuteComplex_DeleteAssociatedToManualPaymentByWorkflowInstanceID,
        HAR_ExecuteComplex_UpdateHarReportWithUpload,
        ShortNonPaymentMngt_ExecuteComplex_DeleteExtractedByPeriodID,
        AdjustmentFiles_ExecuteComplex_DeleteIATAClawbacks,
        AgentInfoFiles_ExecuteComplex_ClearContent,
        AdjustmentFiles_ExecuteComplex_PrepareClawbacks,
        Clawbacks_ExecuteComplex_PostClawbacksPrepareClawbacks,
        Clawbacks_ExecuteComplex_SwitchInclusionStatusByID,

        Periods_ExecuteComplex_ReOpenPeriod,
        Clawbacks_ExecuteComplex_ManuallyConfirm,
        Clawbacks_ExecuteComplex_Reject,
        ClientBalances_ExecuteComplex_UpdatePreviousBalancesForNextPeriod,
        Clawbacks_ExecuteComplex_UpdateADCMNumber,
        ClientBalances_ExecuteComplex_CreateMissingForNextPeriod,
        Clients_ExecuteComplex_ImportAirlines,
        Clients_ExecuteComplex_ImportBankingInfo,
        BankStatementFiles_ExecuteComplex_UndoSanityCheck,
        Clawbacks_ExecuteComplex_ImportClawback,
        ManualMatching_ExecuteComplex_Import,
        SettlementAdjustments_ExecuteComplex_Import,
        CompensationMgt_ExecuteComplex_ImportCompensationFileUnParsed,
        CompensationMgt_ExecuteComplex_ImportCompensationFileDelete,
        CompensationMgt_ExecuteComplex_ImportCompensationFileComplete,
        CompensationMgt_ExecuteComplex_ImportCompensationFileCheck,
        ManualMatching_ExecuteComplex_AssociateWithBankMovement,
        ManualMatching_ExecuteComplex_MoveImportToSnapshotIDs,
        HAR_ExecuteComplex_SubmitForTopman,
        Clawbacks_ExecuteComplex_SetIATAClawbackPeriod,
        AdjustmentFiles_ExecuteComplex_DeleteSendAdjustmentFiles,
        AdjustmentFiles_ExecuteComplex_RejectSendAdjustmentFilesControl,
        ClientBalances_ExecuteComplex_UpdateCBFromPeriodMngt,
        BankDirectoryPlus_ExecuteComplex_ImportFile,
        BankDirectoryPlus_ExecuteComplex_ImportFile_Add,
        BankDirectoryPlus_ExecuteComplex_ImportFile_Modify,
        BankDirectoryPlus_ExecuteComplex_ImportFile_Delete,
        IBANStructure_ExecuteComplex_ImportFile,
        IBANStructure_ExecuteComplex_ImportFile_Add,
        IBANStructure_ExecuteComplex_ImportFile_Modify,
        IBANStructure_ExecuteComplex_ImportFile_Delete,
        Sections_SectionBankingMsgConfig_ExecuteComplex_ReorganizeLevelOrderIndex,
        Sections_SectionBankingMsgConfig_ExecuteComplex_UpdateAssociationOrder,
        Sections_ExecuteComplex_GetImportTemplateForBankingMsgConfig,
        AccountInfo_ExecuteComplex_UpdateHingeAccountFields,
        BankDirectoryPlus_ExecuteComplex_CleanupImportFile,

        HAR_ExecuteComplex_UpdateHARSupportingDocument,

        Matching_ExecuteComplex_UpdateIsBankGuarantee,
        Matching_ExecuteComplex_UpdateIsBankGuaranteeByBankMovAsoc,

        Sections_ExecuteComplex_SetUsesDirectDebitAgents,

        Sections_ExecuteComplex_GetNextIBUFileSequenceNumber,
        AdjustmentFiles_ExecuteComplex_GetCASSClawbacksDataForIBUFile,

        //Discrepancy
        RemDescReports_ExecuteComplex_Generate,
        RemDescReports_ExecuteComplex_GetDataForTxtReport,
        RemDescReports_ExecuteComplex_GetDataForXlsReport,
        RemDescReports_ExecuteComplex_UpdateSendStatus,
    }

    public enum TestingCallID
    {
        Workflow_GetWFAIForWorkflowCode,
        Clawbacks_GetClawbacksForPeriodID,
        GetClientsForSectionID,
        GetHeadOfficeForSectionID,
        
    }

    #endregion

    #region " Public Classes "

    public class SwiftValidationFilePrefix
    {
        public const string BANK_DIRECTORY_PLUS = "BANKDIRECTORYPLUS";
        public const string IBAN_PLUS = "IBANSTRUCTURE";
    }

    public class AIMSConstants
    {
        public const string HEADER = "IATA CODE;CASS CODE;CLASS;LOCATION TYPE;LEGAL  NAME;TRADING  NAME;ADDRESS;CITY;POSTAL CODE;COUNTRY;MAILING ADDRESS;MAILING POST CODE;MAILING CITY;MAILING COUNTRY;AGT_ISD;TEL;FAX;E-MAIL;WEB SITE;CONTACT NAME;GEOCTRY_CODE;X-REF1;CASS CODE1;X-REF1 TYPE;X-REF2;CASS CODE2;XREF2LOC TYPE;STATE;AGT_CTRY;APT_CRN_STAT;APT_CRN_REAS;AGT_APPR_DT;APT_CRN_DT;AGT_TAX_R1;DEFAULT DATE";
        public const int FILENAME_MAX_LENGTH = 217;
    }

    /// <summary>
    /// This class contains constants used on the server-side application
    /// </summary>
    public class azConstants
    {
        public const long NullID = -1;
        public const int MaxFileSize = 1048576;

        public static string BusinessTaskID
        {
            get
            {
                return ConfigurationManager.AppSettings["BusinessTaskID"];
            }
        }

        public static string SecurityBusinessTaskID
        {
            get
            {
                return ConfigurationManager.AppSettings["SecurityBusinessTaskID"];
            }
        }
    }

    public class azConfigFileWSInfo
    {
        public const string URL = "/Location/@url";
    }

    public class azWebServices
    {
        public const string SecurityService = "SecurityService";
        public const string IATARSService = "IATARSService";

    }

    /// <summary>
    /// This class contains constants used as identifiers of the server-side cookies
    /// </summary>
    public class CookieNames
    {
        public const string CulturePref = "CulturePref";
        public const string Username = "Username";
        public const string TimezoneDiff = "TimezoneDiff";
    }

    /// <summary>
    /// This class contains the unique identifiers of PickLists
    /// </summary>
    public class PickLists
    {
        public const string ACCOUNTNUMBERLENGTH = "ACCOUNTNUMBERLENGTH";
        public const string ACCOUNTWITHINSTITUTIONOPT = "ACCOUNTWITHINSTITUTIONOPT";
        public const string ACTIONTYPE = "ACTIONTYPE";
        public const string ACTIVITYCATEGORY = "ACTIVITYCATEGORY";
        public const string ACTIVITYSEVERITY = "ACTIVITYSEVERITY";
        public const string ADJUSTMENTCONFSTATUS = "ADJUSTMENTCONFSTATUS";
        public const string ADJUSTMENTFILEDETAILTYPE = "ADJUSTMENTFILEDETAILTYPE";
        public const string ADJUSTMENTFILESSTATUS = "ADJUSTMENTFILESSTATUS";
        public const string ADJUSTMENTFILETYPE = "ADJUSTMENTFILETYPE";
        public const string ADJUSTMENTSTATUS = "ADJUSTMENTSTATUS";
        public const string ADJUSTMENTTYPE = "ADJUSTMENTTYPE";
        public const string APPROVALSTATUS = "APPROVALSTATUS";
        public const string AUTOMATCHINGTYPE = "AUTOMATCHINGTYPE";
        public const string BALANCEENTRYTYPE = "BALANCEENTRYTYPE";
        public const string BALANCESTATUS = "BALANCESTATUS";
        public const string BANCHCODES = "BANCHCODES";
        public const string BANKINGSERVICINGOPTIONS = "BANKINGSERVICINGOPTIONS";
        public const string BANKINTERMEDIARYOPTION = "BANKINTERMEDIARYOPTION";
        public const string BANKMESSAGEINSTRUCTIONS = "BANKMESSAGEINSTRUCTIONS";
        public const string BANKMESSAGES = "BANKMESSAGES";
        public const string BANKMOVEMENTTYPE = "BANKMOVEMENTTYPE";
        public const string BANKSTATEMENTTYPE = "BANKSTATEMENTTYPE";
        public const string BANKTYPE = "BANKTYPE";
        public const string BICTERMINALCODES = "BICTERMINALCODES";
        public const string BILLINGFILEFORMAT = "BILLINGFILEFORMAT";
        public const string BILLINGFILESSTATUS = "BILLINGFILESSTATUS";
        public const string BSRECEPTIONSTATUS = "BSRECEPTIONSTATUS";
        public const string BSPARTSTATUS = "BSPARTSTATUS";
        public const string BSTRANSACTIONTYPE = "BSTRANSACTIONTYPE";
        public const string CHANGETRACKINGFIELDS = "CHANGETRACKINGFIELDS";
        public const string CHANGETRACKINGTABLES = "CHANGETRACKINGTABLES";
        public const string CLIENTACCINSTRUCTINGCODE = "CLIENTACCINSTRUCTINGCODE";
        public const string CLIENTBALANCEGROUPBY = "CLIENTBALANCEGROUPBY";
        public const string CLIENTBANKACCOUNTSTATUS = "CLIENTBANKACCOUNTSTATUS";
        public const string CLIENTBANKACCOUNTTYPE = "CLIENTBANKACCOUNTTYPE";
        public const string CLIENTTYPE = "CLIENTTYPE";
        public const string CONTROLSTATUS = "CONTROLSTATUS";
        public const string COUNTRYSALESFORCE = "COUNTRYSALESFORCE";
        public const string DASHBOARDDISPLAY = "DASHBOARDDISPLAY";
        public const string DATAIMPORTATIONSTATUS = "DATAIMPORTATIONSTATUS";
        public const string DATAIMPORTATIONTYPE = "DATAIMPORTATIONTYPE";
        public const string DATEEVENT = "DATEEVENT";
        public const string DEBITCREDITMARKTYPE = "DEBITCREDITMARKTYPE";
        public const string DETAILSOFCHARGE = "DETAILSOFCHARGE";
        public const string DOCUMENTTYPE = "DOCUMENTTYPE";
        public const string ENTITYSUBTYPE = "ENTITYSUBTYPE";
        public const string ENTITYTYPE = "ENTITYTYPE";
        public const string ERRORTYPE = "ERRORTYPE";
        public const string FILEACTENCODING = "FILEACTENCODING";
        public const string FILEACTLIMITS = "FILEACTLIMITS";
        public const string FILESTATUS = "FILESTATUS";
        public const string FTPSECURITY = "FTPSECURITY";
        public const string HARPACKAGESTATUS = "HARPACKAGESTATUS";
        public const string ICCSREPORTSTATUS = "ICCSREPORTSTATUS";
        public const string ICCSREPORTTYPE = "ICCSREPORTTYPE";
        public const string INCLUSIONSTATUS = "INCLUSIONSTATUS";
        public const string LANGUAGE = "LANGUAGE";
        public const string LOGTYPE = "LOGTYPE";
        public const string MANUALMATCHSTATUS = "MANUALMATCHSTATUS";
        public const string MATCHINGRULETYPE = "MATCHINGRULETYPE";
        public const string MATCHINGTYPE = "MATCHINGTYPE";
        public const string MATCHSTATUS = "MATCHSTATUS";
        public const string MESSAGEENTITYTYPE = "MESSAGEENTITYTYPE";
        public const string MESSAGETRANSMISSIONTYPE = "MESSAGETRANSMISSIONTYPE";
        public const string NEGATIVEREMITTANCEACTION = "NEGATIVEREMITTANCEACTION";
        public const string NEWSETTLEMENTTYPE = "NEWSETTLEMENTTYPE";
        public const string OPERATIONSTATUS = "OPERATIONSTATUS";
        public const string OPERATIONTYPES = "OPERATIONTYPES";
        public const string ORDERINGCUSTFORMAT = "ORDERINGCUSTFORMAT";
        
        //Discrepancy
        public const string OUTBOUNDFILESTATUS = "OUTBOUNDFILESTATUS";

        public const string OUTGOINGSYSTEM = "OUTGOINGSYSTEM";
        public const string PAYMENTFILECODES = "PAYMENTFILECODES";
        public const string PAYMENTINSTFILESTATUS = "PAYMENTINSTFILESTATUS";
        public const string PAYMENTINSTFILETYPE = "PAYMENTINSTFILETYPE";
        public const string PERIOD = "PERIOD";
        public const string PERIODSTATE = "PERIODSTATE";
        public const string PERMISSION = "PERMISSION";
        public const string PROCESS = "PROCESS";
        public const string PROCESSACTIVITYTYPE = "PROCESSACTIVITYTYPE";
        public const string PROCESSMODULE = "PROCESSMODULE";
        public const string PRORATIONEXCLUSIONTYPE = "PRORATIONEXCLUSIONTYPE";
        public const string PRORATIONMETHODS = "PRORATIONMETHODS";
        public const string PRORATIONPLANSTATUS = "PRORATIONPLANSTATUS";
        public const string PRORATIONREASON = "PRORATIONREASON";
        public const string PRORATIONTYPE = "PRORATIONTYPE";
        public const string RSPROCESSSTATUS = "RSPROCESSSTATUS";
        
        //Discrepancy
        public const string REMDESCREPORTTYPE = "REMDESCREPORTTYPE";

        public const string SALESFORCECASESTATUS = "SALESFORCECASESTATUS";
        public const string SANITYCHECKSTATUS = "SANITYCHECKSTATUS";
        public const string SECURITYGROUPS = "SECURITYGROUPS";
        public const string SETTLEMENTSTATUS = "SETTLEMENTSTATUS";
        public const string SETTLEMENTTYPE = "SETTLEMENTTYPE";
        public const string STATEMENTRECEPTIONNUMBER = "STATEMENTRECEPTIONNUMBER";
        public const string SUBCLIENTTYPE = "SUBCLIENTTYPE";
        public const string SYSTEMDATE = "SYSTEMDATE";
        public const string SYSTEMDATETYPE = "SYSTEMDATETYPE";
        public const string SYSTEMTYPE = "SYSTEMTYPE";
        public const string TRANSACTIONTYPE = "TRANSACTIONTYPE";
        public const string TRANSACTIONWARNINGTYPE = "TRANSACTIONWARNINGTYPE";
        public const string TRANSFERTYPE = "TRANSFERTYPE";
        public const string UNALLOCATEDTYPE = "UNALLOCATEDTYPE";
        public const string WORKFLOWACTIVITYTYPE = "WORKFLOWACTIVITYTYPE";
        public const string WORKFLOWSTAGE = "WORKFLOWSTAGE";
        public const string WORKFLOWSTATUS = "WORKFLOWSTATUS";
        public const string YESNO = "YESNO";        
    }

    /// <summary>
    /// his class contains the unique identifiers of PickLists Value codes
    /// </summary>
    public class PickListValueCodes
    {
        public const string ACCOUNTNUMBERLENGTH_VARIABLE = "VARIABLE";

        public const string ACCOUNTNUMBERLENGTH_FIXED = "FIXED";

        public const string ACCOUNTWITHINSTITUTION_A = "A";

        public const string ACCOUNTWITHINSTITUTION_C = "C";

        public const string ACCOUNTWITHINSTITUTION_D = "D";

        public const string ACTIONTYPE_NONE = "NONE";

        public const string ACTIONTYPE_EXCESS = "EXCESS";

        public const string ACTIONTYPE_SHORT = "SHORT";

        public const string ACTIONTYPE_EXCESSSHORT = "EXCESSSHORT";

        public const string ACTIONTYPE_DEFAULT = "DEFAULT";

        public const string ACTIONTYPE_DISPUTE = "DISPUTE";

        public const string ACTIONTYPE_RECOVERY = "RECOVERY";

        public const string ACTIVITYCATEGORY_HUB = "HUB";

        public const string ACTIVITYCATEGORY_SECTIONS = "SECTIONS";

        public const string ACTIVITYCATEGORY_PERIODS = "PERIODS";

        public const string ACTIVITYCATEGORY_WORKFLOW = "WORKFLOW";

        public const string ACTIVITYCATEGORY_INBOUND = "INBOUND";

        public const string ACTIVITYCATEGORY_OUTBOUND = "OUTBOUND";

        public const string ACTIVITYCATEGORY_SECURITY = "SECURITY";

        public const string ACTIVITYCATEGORY_FIELDCHANGES = "FIELDCHANGES";

        public const string ACTIVITYCATEGORY_CONTROLS = "CONTROLS";

        public const string ACTIVITYSEVERITY_INFORMATION = "INFORMATION";

        public const string ACTIVITYSEVERITY_WARNING = "WARNING";

        public const string ACTIVITYSEVERITY_ERROR = "ERROR";

        public const string ADJUSTMENTCONFSTATUS_PENDING = "PENDING";

        public const string ADJUSTMENTCONFSTATUS_CONFIRMED = "CONFIRMED";

        public const string ADJUSTMENTCONFSTATUS_UNCONFIRMED = "UNCONFIRMED";

        public const string ADJUSTMENTCONFSTATUS_REISSUED = "REISSUED";

        public const string ADJUSTMENTCONFSTATUS_REJECTED = "REJECTED";

        public const string ADJUSTMENTFILEDETAILTYPE_SPCR = "SPCR";

        public const string ADJUSTMENTFILEDETAILTYPE_SPDR = "SPDR";

        public const string ADJUSTMENTFILEDETAILTYPE_ACMD = "ACMD";

        public const string ADJUSTMENTFILEDETAILTYPE_ADMD = "ADMD";

        public const string ADJUSTMENTFILEDETAILTYPE_BTA = "BTA";

        public const string ADJUSTMENTFILEDETAILTYPE_IBU = "IBU";

        public const string ADJUSTMENTFILESSTATUS_NOTSENT = "NOTSENT";

        public const string ADJUSTMENTFILESSTATUS_SENT = "SENT";

        public const string ADJUSTMENTFILESSTATUS_SENT_SUCCESS = "SENT_SUCCESS";

        public const string ADJUSTMENTFILESSTATUS_SENT_ERROR = "SENT_ERROR";

        public const string ADJUSTMENTFILETYPE_BSP = "BSP";

        public const string ADJUSTMENTFILETYPE_CASS = "CASS";

        public const string ADJUSTMENTSTATUS_NEW = "NEW";

        public const string ADJUSTMENTSTATUS_PENDINGAPPROVAL = "PENDINGAPPROVAL";

        public const string ADJUSTMENTSTATUS_APPROVED = "APPROVED";

        public const string ADJUSTMENTSTATUS_CANCELED = "CANCELED";

        public const string ADJUSTMENTSTATUS_REJECTED = "REJECTED";

        public const string ADJUSTMENTTYPE_NONREMITTANCE = "NONREMITTANCE";

        public const string ADJUSTMENTTYPE_SHORT = "SHORT";

        public const string ADJUSTMENTTYPE_EXCESS = "EXCESS";

        public const string ADJUSTMENTTYPE_DEFAULT = "DEFAULT";

        public const string ADJUSTMENTTYPE_DISPUTE = "DISPUTE";

        public const string ADJUSTMENTTYPE_RECOVERY = "RECOVERY";

        public const string APPROVALSTATUS_MODIFIED = "MODIFIED";

        public const string APPROVALSTATUS_APPROVED = "APPROVED";

        public const string APPROVALSTATUS_REJECTED = "REJECTED";

        public const string AUTOMATCHINGTYPE_FINALMATCHING = "FINALMATCHING";

        public const string AUTOMATCHINGTYPE_SOFTMATCHING = "SOFTMATCHING";

        public const string BALANCEENTRYTYPE_AUTOMATCH = "AUTOMATCH";

        public const string BALANCEENTRYTYPE_MANUALMATCH = "MANUALMATCH";

        public const string BALANCEENTRYTYPE_COMPENSATION = "COMPENSATION";

        public const string BALANCEENTRYTYPE_CLAWBACK = "CLAWBACK";

        public const string BALANCEENTRYTYPE_SETTLEMENTADJUSTMENT = "SETTLEMENTADJUSTMENT";

        public const string BALANCEENTRYTYPE_MANUALSETTLEMENTADJ = "MANUALSETTLEMENTADJ";

        public const string BALANCESTATUS_BALANCED = "BALANCED";

        public const string BALANCESTATUS_UNBALANCED = "UNBALANCED";

        public const string BALANCESTATUS_EXCESS = "EXCESS";

        public const string BALANCESTATUS_PENDING = "PENDING";

        public const string BANCHCODES_AMM = "AMM";

        public const string BANCHCODES_BJS = "BJS";

        public const string BANCHCODES_MAD = "MAD";

        public const string BANCHCODES_MIA = "MIA";

        public const string BANCHCODES_SIN = "SIN";

        public const string BANKINGSERVICINGOPTIONS_A = "A";

        public const string BANKINGSERVICINGOPTIONS_C = "C";

        public const string BANKINTERMEDIARYOPTION_A = "A";

        public const string BANKINTERMEDIARYOPTION_C = "C";

        public const string BANKINTERMEDIARYOPTION_D = "D";

        public const string BANKMESSAGEINSTRUCTIONS_SINGLEPAYMENT = "SINGLEPAYMENT";

        public const string BANKMESSAGEINSTRUCTIONS_MULTIPLEPAYMENT = "MULTIPLEPAYMENT";

        public const string BANKMESSAGES_MT101 = "MT101";

        public const string BANKMESSAGES_MT940 = "MT940";

        public const string BANKMESSAGES_MT942 = "MT942";

        public const string BANKSTATEMENTTYPE_XLS = "XLS";

        public const string BANKMESSAGES_MT010 = "MT010";

        public const string BANKMESSAGES_MT011 = "MT011";

        public const string BANKMESSAGES_MT096 = "MT096";

        public const string BANKMOVEMENTTYPE_NMSC = "NMSC";

        public const string BANKMOVEMENTTYPE_NTRF = "NTRF";

        public const string BANKSTATEMENTTYPE_940 = "940";

        public const string BANKSTATEMENTTYPE_941 = "941";

        public const string BANKSTATEMENTTYPE_942 = "942";

        public const string BANKTYPE_SWIFT = "SWIFT";

        public const string BANKTYPE_NONSWIFT = "NONSWIFT";

        public const string BICTERMINALCODES_X = "X";

        public const string BICTERMINALCODES_A = "A";

        public const string BICTERMINALCODES_B = "B";

        public const string BILLINGFILEFORMAT_AP2 = "AP2";

        public const string BILLINGFILEFORMAT_EX = "EX";

        public const string BILLINGFILEFORMAT_I = "I";

        public const string BILLINGFILESSTATUS_RECEIVED = "RECEIVED";

        public const string BILLINGFILESSTATUS_ISC_COMPLETED = "ISC_COMPLETED";

        public const string BILLINGFILESSTATUS_RSC_COMPLETED = "RSC_COMPLETED";

        public const string BILLINGFILESSTATUS_PENDING_CONFIRMATION = "PENDING_CONFIRMATION";

        public const string BILLINGFILESSTATUS_ACCEPTED = "ACCEPTED";

        public const string BILLINGFILESSTATUS_REJECTED = "REJECTED";

        public const string BILLINGFILESSTATUS_ERROR = "ERROR";

        public const string BILLINGFILESSTATUS_IGNORED = "IGNORED";

        public const string BILLINGFILESSTATUS_CANCELLED = "CANCELLED";

        public const string BSRECEPTIONSTATUS_INPROGRESS = "INPROGRESS";

        public const string BSRECEPTIONSTATUS_COMPLETED = "COMPLETED";

        public const string BSRECEPTIONSTATUS_ERROR = "ERROR";

        public const string BSPARTSTATUS_OK = "OK";

        public const string BSPARTSTATUS_ERROR = "ERROR";

        public const string BSTRANSACTIONTYPE_S = "S";

        public const string BSTRANSACTIONTYPE_N = "N";

        public const string BSTRANSACTIONTYPE_F = "F";

        public const string CHANGETRACKINGFIELDS_FIELD1 = "FIELD1";

        public const string CHANGETRACKINGTABLES_TABLE1 = "TABLE1";

        public const string CLIENTACCINSTRUCTINGCODE_CHCK = "CHCK";

        public const string CLIENTACCINSTRUCTINGCODE_RPGS = "RPGS";

        public const string CLIENTACCINSTRUCTINGCODE_URGP = "URGP";

        public const string CLIENTBALANCEGROUPBY_CLIENT = "CLIENT";

        public const string CLIENTBALANCEGROUPBY_GROUP = "GROUP";

        public const string CLIENTBANKACCOUNTSTATUS_PENDINGAPPROVAL = "PENDINGAPPROVAL";

        public const string CLIENTBANKACCOUNTSTATUS_APPROVED = "APPROVED";

        public const string CLIENTBANKACCOUNTSTATUS_REJECTED = "REJECTED";

        public const string CLIENTBANKACCOUNTTYPE_STD = "STD";

        public const string CLIENTBANKACCOUNTTYPE_DD = "DD";

        public const string CLIENTTYPE_NONE = "NONE";

        public const string CLIENTTYPE_AGENT = "AGENT";

        public const string CLIENTTYPE_AIRLINE = "AIRLINE";

        public const string CLIENTTYPE_OTHER = "OTHER";

        public const string CONTROLSTATUS_PENDING = "PENDING";

        public const string CONTROLSTATUS_APPROVED = "APPROVED";

        public const string CONTROLSTATUS_REJECTED = "REJECTED";

        public const string COUNTRYSALESFORCE_0 = "0";

        public const string COUNTRYSALESFORCE_1 = "1";

        public const string COUNTRYSALESFORCE_2 = "2";

        public const string COUNTRYSALESFORCE_3 = "3";

        public const string COUNTRYSALESFORCE_4 = "4";

        public const string COUNTRYSALESFORCE_5 = "5";

        public const string COUNTRYSALESFORCE_6 = "6";

        public const string COUNTRYSALESFORCE_7 = "7";

        public const string COUNTRYSALESFORCE_8 = "8";

        public const string COUNTRYSALESFORCE_9 = "9";

        public const string COUNTRYSALESFORCE_10 = "10";

        public const string DASHBOARDDISPLAY_GLOBALDASHBOARD = "GLOBALDASHBOARD";

        public const string DASHBOARDDISPLAY_PERIODDASHBOARD = "PERIODDASHBOARD";

        public const string DASHBOARDDISPLAY_OPERATIONDASHBOARD = "OPERATIONDASHBOARD";

        public const string DASHBOARDDISPLAY_HUBDASHBOARD = "HUBDASHBOARD";

        public const string DATAIMPORTATIONSTATUS_INPROGRESS = "INPROGRESS";

        public const string DATAIMPORTATIONSTATUS_COMPLETED = "COMPLETED";

        public const string DATAIMPORTATIONSTATUS_ERROR = "ERROR";

        public const string DATAIMPORTATIONSTATUS_COMPLETED_ERROR = "COMPLETED_ERROR";

        public const string DATAIMPORTATIONSTATUS_CANCELLED = "CANCELLED";

        public const string DATAIMPORTATIONTYPE_BSPLINK = "BSPLINK";

        public const string DATAIMPORTATIONTYPE_CASSLINK = "CASSLINK";

        public const string DATAIMPORTATIONTYPE_AGENTINFO = "AGENTINFO";

        public const string DATAIMPORTATIONTYPE_AIRLINEINFO = "AIRLINEINFO";

        public const string DATAIMPORTATIONTYPE_BANKSTATEMENTS = "BANKSTATEMENTS";

        public const string DATAIMPORTATIONTYPE_EXCEPSTATUS = "EXCEPSTATUS";

        public const string DATEEVENT_SALESPERIOD = "SALESPERIOD";

        public const string DATEEVENT_BILLINGPROCESSING = "BILLINGPROCESSING";

        public const string DATEEVENT_BILLINGPRESENTATION = "BILLINGPRESENTATION";

        public const string DATEEVENT_RSSETTLEMENT = "RSSETTLEMENT";

        public const string DATEEVENT_REMITTANCE = "REMITTANCE";

        public const string DEBITCREDITMARKTYPE_C = "C";

        public const string DEBITCREDITMARKTYPE_D = "D";

        public const string DEBITCREDITMARKTYPE_RC = "RC";

        public const string DEBITCREDITMARKTYPE_RD = "RD";

        public const string DEBITCREDITMARKTYPE_EC = "EC";

        public const string DEBITCREDITMARKTYPE_ED = "ED";

        public const string DETAILSOFCHARGE_BEN = "BEN";

        public const string DETAILSOFCHARGE_OUR = "OUR";

        public const string DETAILSOFCHARGE_SHA = "SHA";

        public const string DOCUMENTTYPE_NONE = "NONE";

        public const string DOCUMENTTYPE_NET = "NET";

        public const string DOCUMENTTYPE_PAY = "PAY";

        public const string DOCUMENTTYPE_COM = "COM";

        public const string DOCUMENTTYPE_PRO = "PRO";

        public const string DOCUMENTTYPE_DIS = "DIS";

        public const string DOCUMENTTYPE_RCV = "RCV";

        public const string DOCUMENTTYPE_ADJ = "ADJ";

        public const string DOCUMENTTYPE_ACM = "ACM";

        public const string ENTITYSUBTYPE_AGENT = "AGENT";

        public const string ENTITYSUBTYPE_AIRLINE = "AIRLINE";

        public const string ENTITYSUBTYPE_OTHER = "OTHER";

        public const string ENTITYTYPE_CLIENT = "CLIENT";

        public const string ENTITYTYPE_HINGE = "HINGE";

        public const string ENTITYTYPE_HUB = "HUB";

        public const string ERRORTYPE_TYPE1 = "TYPE1";

        public const string ERRORTYPE_TYPE2 = "TYPE2";

        public const string FILEACTENCODING_ASCII = "ASCII";

        public const string FILEACTENCODING_UTF8 = "UTF8";

        public const string FILEACTLIMITS_DATASIZE = "DATASIZE";

        public const string FILEACTLIMITS_CHAR = "CHAR";

        public const string FILEACTLIMITS_LINES = "LINES";

        public const string FILESTATUS_APPROVED = "APPROVED";

        public const string FILESTATUS_REJECTED = "REJECTED";

        public const string FILESTATUS_PENDINGCONFIRMATION = "PENDINGCONFIRMATION";

        public const string FILESTATUS_ERROR = "ERROR";

        public const string FTPSECURITY_FTPS = "FTPS";

        public const string FTPSECURITY_SFTP = "SFTP";

        public const string HAR_CREATED = "HAR_CREATED";

        public const string HAR_PENDING = "HAR_PENDING";

        public const string HAR_SUBMITTED_FOR_TOPMAN = "HAR_SUBMITTED_FOR_TOPMAN";

        public const string HAR_SUBMITTED_FOR_APPROVAL = "HAR_SUBMITTED_FOR_APPROVAL";

        public const string HAR_APPROVED = "HAR_APPROVED";

        public const string HAR_REJECTED = "HAR_REJECTED";

        public const string ICCSREPORTSTATUS_CREATED = "CREATED";

        public const string ICCSREPORTSTATUS_SENT = "SENT";

        public const string ICCSREPORTSTATUS_SENTSUCCESSFULLY = "SENTSUCCESSFULLY";

        public const string ICCSREPORTSTATUS_SENTERROR = "SENTERROR";

        public const string ICCSREPORTTYPE_PROVISIONARY = "PROVISIONARY";

        public const string ICCSREPORTTYPE_FINAL = "FINAL";

        public const string INCLUSIONSTATUS_INCLUDED = "INCLUDED";

        public const string INCLUSIONSTATUS_EXCLUDED = "EXCLUDED";

        public const string LANGUAGE_NONE = "NONE";

        public const string LANGUAGE_ENGLISH = "ENGLISH";

        public const string LANGUAGE_FRENCH = "FRENCH";

        public const string LANGUAGE_SPANISH = "SPANISH";

        public const string LOGTYPE_INF = "INF";

        public const string LOGTYPE_ERR = "ERR";

        public const string MATCHINGRULETYPE_LOCATIONNET = "LOCATIONNET";

        public const string MATCHINGRULETYPE_GROUPNET = "GROUPNET";

        public const string MATCHINGRULETYPE_IATACODE = "IATACODE";

        public const string MATCHINGRULETYPE_SHORTNAME = "SHORTNAME";

        public const string MATCHINGRULETYPE_LOCATIONNETROUNDED = "LOCATIONNETROUNDED";

        public const string MATCHINGRULETYPE_GROUPNETROUNDED = "GROUPNETROUNDED";

        public const string MATCHINGRULETYPE_LOCATIONPENDING = "LOCATIONPENDING";

        public const string MATCHINGRULETYPE_GROUPPENDING = "GROUPPENDING";

        public const string MATCHINGRULETYPE_LOCATIONPENDINGROUNDED = "LOCATIONPENDINGROUNDED";

        public const string MATCHINGRULETYPE_GROUPPENDINGROUNDED = "GROUPPENDINGROUNDED";

        public const string MATCHINGRULETYPE_LOCATIONADJUSTMENTNET = "LOCATIONADJUSTMENTNET";

        public const string MATCHINGTYPE_AUTOMATIC = "AUTOMATIC";

        public const string MATCHINGTYPE_MANUAL = "MANUAL";

        public const string MATCHSTATUS_UNCONFIRMED = "UNCONFIRMED";

        public const string MATCHSTATUS_CANCELED = "CANCELED";

        public const string MATCHSTATUS_APPROVED = "APPROVED";

        public const string MESSAGEENTITYTYPE_NONE = "NONE";

        public const string MESSAGEENTITYTYPE_OPERATION = "OPERATION";

        public const string MESSAGEENTITYTYPE_AGENT = "AGENT";

        public const string MESSAGEENTITYTYPE_AIRLINE = "AIRLINE";

        public const string MESSAGEENTITYTYPE_OTHER = "OTHER";

        public const string MESSAGEENTITYTYPE_HUB = "HUB";

        public const string MESSAGETRANSMISSIONTYPE_FIN = "FIN";

        public const string MESSAGETRANSMISSIONTYPE_FILEACT = "FILEACT";

        public const string NEGATIVEREMITTANCEACTION_CREATEADJUSTMENT = "CREATEADJUSTMENT";

        public const string NEGATIVEREMITTANCEACTION_CREATEEFT = "CREATEEFT";

        public const string NEGATIVEREMITTANCEACTION_LEAVEUNBALANCED = "LEAVEUNBALANCED";

        public const string NEWSETTLEMENTTYPE_RS = "RS";

        public const string NEWSETTLEMENTTYPE_FR = "FR";

        public const string NEWSETTLEMENTTYPE_NR = "NR";

        public const string OPERATIONSTATUS_LIVE = "LIVE";

        public const string OPERATIONSTATUS_TEST = "TEST";

        public const string OPERATIONTYPES_E = "E";

        public const string OPERATIONTYPES_I = "I";

        public const string OPERATIONTYPES_C = "C";

        public const string OPERATIONTYPES_L = "L";

        public const string OPERATIONTYPES_Y = "Y";

        public const string ORDERINGCUSTFORMAT_G = "G";

        public const string ORDERINGCUSTFORMAT_H = "H";

        //Discrepancy
        public const string OUTBOUNDFILESTATUS_GENERATED = "GENERATED";

        public const string OUTBOUNDFILESTATUS_PENDINGAPPROVAL = "PENDINGAPPROVAL";
        public const string OUTBOUNDFILESTATUS_APPROVED = "APPROVED";
        public const string OUTBOUNDFILESTATUS_CANCELED = "CANCELED";
        public const string OUTBOUNDFILESTATUS_REJECTED = "REJECTED";

        public const string OUTBOUNDFILESTATUS_SENT = "SENT";
        public const string OUTBOUNDFILESTATUS_SENTSUCCESSFULLY = "SENTSUCCESSFULLY";
        public const string OUTBOUNDFILESTATUS_SENTERROR = "SENTERROR";
        
        public const string OUTGOINGSYSTEM_BANKINGCONNECTIVITY = "BANKINGCONNECTIVITY";

        public const string OUTGOINGSYSTEM_BSPLINK = "BSPLINK";

        public const string OUTGOINGSYSTEM_CASSLINK = "CASSLINK";

        public const string OUTGOINGSYSTEM_DISCREPANCY = "DISCREPANCY";

        public const string OUTGOINGSYSTEM_ICCS = "ICCS";

        public const string OUTGOINGSYSTEM_SIDRA = "SIDRA";

        public const string PAYMENTFILECODES_PAYLOAD = "PAYLOAD";

        public const string PAYMENTFILECODES_PARAMETER = "PARAMETER";

        public const string PAYMENTFILECODES_ACK = "ACK";

        public const string PAYMENTFILECODES_NAK = "NAK";

        public const string PAYMENTFILECODES_RMA = "RMA";

        public const string PAYMENTFILECODES_FOR = "FOR";

        public const string PAYMENTFILECODES_ERR = "ERR";

        public const string PAYMENTFILECODES_MT010 = "MT010";

        public const string PAYMENTFILECODES_MT011 = "MT011";

        public const string PAYMENTFILECODES_MT199 = "MT199";

        public const string PAYMENTFILECODES_NOTDEL = "NOTDEL";

        public const string PAYMENTFILECODES_FAILED = "FAILED";

        public const string PAYMENTFILECODES_REC = "REC";

        public const string PAYMENTFILECODES_NOT = "NOT";

        public const string PAYMENTFILECODES_PAIN002 = "PAIN002";

        public const string PAYMENTINSTFILESTATUS_GENERATED = "GENERATED";
        public const string PAYMENTINSTFILESTATUS_SENT = "SENT";
        public const string PAYMENTINSTFILESTATUS_SENT_ACK = "SENT_ACK";
        public const string PAYMENTINSTFILESTATUS_SENT_NACK = "SENT_NACK";
        public const string PAYMENTINSTFILESTATUS_SENT_BIZTALK_ACK = "SENT_BIZTALK_ACK";
        public const string PAYMENTINSTFILESTATUS_SENT_BIZTALK_NACK = "SENT_BIZTALK_NACK";
        public const string PAYMENTINSTFILESTATUS_SENT_RECOVERED_FROM_DR = "SENT_RECOVERED_FROM_DR";

        public const string PAYMENTINSTFILETYPE_MT101 = "MT101";

        public const string PERIOD_2011_12A = "2011_12A";

        public const string PERIOD_2011_12B = "2011_12B";

        public const string PERIOD_2011_12C = "2011_12C";

        public const string PERIOD_2011_12D = "2011_12D";

        public const string PERIODSTATE_NOTSTARTED = "NOTSTARTED";
        public const string PERIODSTATE_ONGOING = "ONGOING";
        public const string PERIODSTATE_CLOSED = "CLOSED";

        public const string PERMISSION_READONLY = "READONLY";
        public const string PERMISSION_EXECUTE = "EXECUTE";
        public const string PERMISSION_CANCANCEL = "CANCANCEL";
        public const string PERMISSION_CANSTART = "CANSTART";

        public const string PROCESS_FUNDSRECEIVED = "FUNDSRECEIVED";

        public const string PROCESS_REPORTEDSALES = "REPORTEDSALES";

        public const string PROCESSACTIVITYTYPE_AUTOMATICPROCESS = "AUTOMATICPROCESS";

        public const string PROCESSACTIVITYTYPE_MANUALPROCESS = "MANUALPROCESS";

        public const string PROCESSACTIVITYTYPE_AUTOMATICCONTROL = "AUTOMATICCONTROL";

        public const string PROCESSACTIVITYTYPE_MANUALCONTROL = "MANUALCONTROL";

        public const string PROCESSMODULE_REMITTANCE = "REMITTANCE";

        public const string PROCESSMODULE_MATCHING = "MATCHING";

        public const string PROCESSMODULE_PRORATIONADJUSTMENT = "PRORATIONADJUSTMENT";

        public const string PROCESSMODULE_HAR = "HAR";

        public const string PROCESSMODULE_DISCREPANCYFOLLOWUP = "DISCREPANCYFOLLOWUP";

        public const string PROCESSMODULE_SETTLEMENT = "SETTLEMENT";

        public const string PRORATIONEXCLUSIONTYPE_DISPUTE = "DISPUTE";

        public const string PRORATIONEXCLUSIONTYPE_BANKGUARANTEE = "BANKGUARANTEE";

        public const string PRORATIONMETHODS_ACCUMULATED = "ACCUMULATED";

        public const string PRORATIONMETHODS_PROGRESSIVE = "PROGRESSIVE";

        public const string PRORATIONPLANSTATUS_MODIFIED = "MODIFIED";

        public const string PRORATIONPLANSTATUS_SUBMITTED = "SUBMITTED";

        public const string PRORATIONPLANSTATUS_APPROVED = "APPROVED";

        public const string PRORATIONPLANSTATUS_REJECTED = "REJECTED";

        public const string PRORATIONREASON_DEFAULT = "DEFAULT";

        public const string PRORATIONREASON_RECOVERY = "RECOVERY";

        public const string PRORATIONREASON_LATEPAYMENT = "LATEPAYMENT";

        public const string PRORATIONTYPE_AGENTLATEPAYMENT = "AGENTLATEPAYMENT";

        public const string PRORATIONTYPE_AIRLINELATEPAYMENT = "AIRLINELATEPAYMENT";

        public const string PRORATIONTYPE_AGENTDEFAULT = "AGENTDEFAULT";

        public const string PRORATIONTYPE_AIRLINESUSPENSION = "AIRLINESUSPENSION";

       
        //Discrepancy
        public const string REMDESCREPORTTYPE_TXT = "TXT";
        public const string REMDESCREPORTTYPE_XLS = "XSL";


        public const string RSPROCESSSTATUS_NOTSTARTED = "NOTSTARTED";

        public const string RSPROCESSSTATUS_INPROGRESS = "INPROGRESS";

        public const string RSPROCESSSTATUS_COMPLETED = "COMPLETED";

        public const string SALESFORCECASESTATUS_NOTSENT = "NOTSENT";

        public const string SALESFORCECASESTATUS_SENT = "SENT";

        public const string SALESFORCECASESTATUS_ERROR = "ERROR";

        public const string SALESFORCECASESTATUS_UPDATED = "UPDATED";

        public const string SANITYCHECKSTATUS_PENDINGCHECK = "PENDINGCHECK";

        public const string SANITYCHECKSTATUS_CHECKFAILED = "CHECKFAILED";

        public const string SANITYCHECKSTATUS_NOTRANSACTIONS = "NOTRANSACTION";

        public const string SANITYCHECKSTATUS_CHECKSUCCESSFUL = "CHECKSUCCESSFUL";

        public const string SANITYCHECKSTATUS_IMPORTCANCELED = "IMPORTCANCELED";

        public const string SANITYCHECKSTATUS_REJECTED = "REJECTED";

        public const string SETTLEMENTSTATUS_PENDINGAPPROVAL = "PENDINGAPPROVAL";

        public const string SETTLEMENTSTATUS_APPROVED = "APPROVED";

        public const string SETTLEMENTSTATUS_REJECTED = "REJECTED";

        public const string SETTLEMENTTYPE_NONE = "NONE";

        public const string SETTLEMENTTYPE_FR = "FR";

        public const string SETTLEMENTTYPE_RS = "RS";

        public const string SETTLEMENTTYPE_WL = "WL";

        public const string SETTLEMENTTYPE_TP = "TP";

        public const string SETTLEMENTTYPE_IATA = "IATA";

        public const string SETTLEMENTTYPE_NR = "NR";

        public const string SUBCLIENTTYPE_INTEREST = "INTEREST";

        public const string SUBCLIENTTYPE_CHARGES = "CHARGES";

        public const string SYSTEMDATE_FIRSTDAY = "FIRSTDAY";

        public const string SYSTEMDATE_LASTDAY = "LASTDAY";

        public const string SYSTEMDATE_REMITTANCEDAY = "REMITTANCEDAY";

        public const string SYSTEMDATE_SETTLEMENTDAY = "SETTLEMENTDAY";

        public const string SYSTEMDATE_HARDAY = "HARDAY";

        public const string SYSTEMDATETYPE_NONE = "NONE";

        public const string SYSTEMDATETYPE_REMITTANCEDAY = "REMITTANCEDAY";

        public const string SYSTEMDATETYPE_SETTLEMENTDAY = "SETTLEMENTDAY";

        public const string SYSTEMDATETYPE_HARDAY = "HARDAY";

        public const string SYSTEMTYPE_NONE = "NONE";

        public const string SYSTEMTYPE_BSP = "BSP";

        public const string SYSTEMTYPE_CASS = "CASS";

        public const string SYSTEMTYPE_CASSCO = "CASSCO";

        public const string SYSTEMTYPE_CASSIM = "CASSIM";

        public const string TRANSACTIONTYPE_NONE = "NONE";

        public const string TRANSACTIONTYPE_SPCR = "SPCR";

        public const string TRANSACTIONTYPE_SPDR = "SPDR";

        public const string TRANSACTIONTYPE_ACMD = "ACMD";

        public const string TRANSACTIONTYPE_ADMD = "ADMD";

        public const string TRANSACTIONWARNINGTYPE_AVERAGECOMPARISON = "AVERAGECOMPARISON";

        public const string TRANSACTIONWARNINGTYPE_SAMEPERIODCOMPARISON = "SAMEPERIODCOMPARISON";

        public const string TRANSACTIONWARNINGTYPE_PREVIOUSPERIODCOMPARISON = "PREVIOUSPERIODCOMPARISON";

        public const string TRANSFERTYPE_BOOKTRANSFER = "BOOKTRANSFER";

        public const string TRANSFERTYPE_DOMESTIC = "DOMESTIC";

        public const string TRANSFERTYPE_CROSSBORDER = "CROSSBORDER";

        public const string TRANSFERTYPE_CHECK = "CHECK";

        public const string TRANSFERTYPE_RTGS = "RTGS";

        public const string WORKFLOWACTIVITYTYPE_AUTOACTIVITY = "AUTOACTIVITY";

        public const string WORKFLOWACTIVITYTYPE_MANUALACTIVITY = "MANUALACTIVITY";

        public const string WORKFLOWACTIVITYTYPE_AUTOCONTROL = "AUTOCONTROL";

        public const string WORKFLOWACTIVITYTYPE_MANUALCONTROL = "MANUALCONTROL";

        public const string WORKFLOWSTAGE_PREPARATION = "PREPARATION";

        public const string WORKFLOWSTAGE_REMITTANCE = "REMITTANCE";

        public const string WORKFLOWSTAGE_PRORATION = "PRORATION";

        public const string WORKFLOWSTAGE_SETTLEMENT = "SETTLEMENT";

        public const string WORKFLOWSTAGE_HAR = "HAR";

        public const string WORKFLOWSTAGE_OTHER = "OTHER";

        public const string WORKFLOWSTATUS_INPROGRESS = "INPROGRESS";

        public const string WORKFLOWSTATUS_FINISHED = "FINISHED";

        public const string WORKFLOWSTATUS_ERROR = "ERROR";

        public const string WORKFLOWSTATUS_CANCELED = "CANCELED";

        public const string WORKFLOWSTATUS_ACCEPTED = "ACCEPTED";

        public const string WORKFLOWSTATUS_REJECTED = "REJECTED";

        public const string WORKFLOWSTATUS_SKIPPED = "SKIPPED";

        public const string WORKFLOWSTATUS_EXECUTING = "EXECUTING";

        public const string WORKFLOWSTATUS_WAITING = "WAITING";

        //SECURITYGROUPS 
        public const string SECURITYGROUPS_SIGNATORY = "SIGNATORY";

        public const string UNALLOCATEDTYPE_CREDIT = "CREDIT";

        public const string UNALLOCATEDTYPE_DEBIT = "DEBIT";
    }

    /// <summary>
    /// This class contains the unique identifiers of Security Access Types
    /// </summary>
    public class AccessType
    {
        public const string Access = "A";
        public const string Modify = "M";
        public const string Create = "C";
        public const string Delete = "D";
        public const string Read = "R";
        public const string FullControl = "FC";
    }

    public class WorkflowCodes
    {
        //Hub level workflow codes
        public const string AGENT_IMPORT = "AGENT_IMPORT";
        public const string AGENT_BANK_INFORMATION_MNGT = "AGENT_BANK_INFORMATION_MNGT";
        public const string AIRLINE_BANK_INFORMATION_MNGT = "AIRLINE_BANK_INFORMATION_MNGT";
        public const string OTHER_BANK_INFO_MNGT = "OTHER_BANK_INFO_MNGT";

        //Operation level workflow codes
        public const string MATCHING = "MATCHING";
        public const string SOFT_MATCHING = "SOFT_MATCHING";
        public const string CLIENT_BALANCE_CONTROL = "CLIENT_BALANCE_CONTROL";

        //Period level workflow codes
        public const string BILLING_FILE_RECEPTION = "BILLING_FILE_RECEPTION";
        public const string REMITTANCE_WRAPPER = "REMITTANCE_WRAPPER";
        public const string PREPARATION_WRAPPER = "PREPARATION_WRAPPER";
        public const string PRORATION_WRAPPER = "PRORATION_WRAPPER";
        public const string SETTLEMENT_WRAPPER = "SETTLEMENT_WRAPPER";
        public const string HAR_WRAPPER = "HAR_WRAPPER";
        public const string SETTLEMENT_RS = "SETTLEMENT_RS";
        public const string SETTLEMENT_FR = "SETTLEMENT_FR";
        public const string SETTLEMENT_NR = "SETTLEMENT_NEGATIVE_REMITTANCE";
        public const string GENERATE_PAYMENT_FILE = "GENERATE_PAYMENT_FILE";
        public const string MASTER_DATA_SNAPSHOT = "MASTER_DATA_SNAPSHOT";
        public const string BANK_STATEMENT_RECEPTION = "BANK_STATEMENT_RECEPTION";

        /*public const string HAR_PACKAGE_RS = "HAR_PACKAGE_RS";
        public const string HAR_PACKAGE_FR = "HAR_PACKAGE_FR";
        public const string HAR_PACKAGE_NR = "HAR_PACKAGE_NR";*/

        public const string HAR_PACKAGE = "HAR_PACKAGE";

        public const string SEND_ADJUSTMENTS = "SEND_ADJUSTMENTS";
        public const string ICCS_PRELIMINARY_REPORT = "ICCS_PRELIMINARY_REPORT";
        public const string ICCS_FINAL_REPORT = "ICCS_FINAL_REPORT";
        public const string NEGATIVE_REMITTANCE_MANAGEMENT = "NEGATIVE_REMITTANCE_MANAGEMENT";
        public const string MANUAL_PAYMENT_MANAGEMENT = "MANUAL_PAYMENT_MANAGEMENT";

        public const string PRORATIONS = "PRORATIONS";
        public const string SHORT_NON_PAYMENT_MANAGEMENT = "SHORT/NON_PAYMENT_MANAGEMENT";

        public const string POST_CLAWBACKS = "POST_CLAWBACKS";
        public const string BILLING_ADJUSTMENTS = "BILLING_ADJUSTMENTS";
    }


    public class ActivityCodes
    {
        //Hub level activity codes
        public const string IMPORT_AGENT_FILE = "IMPORT_AGENT_FILE";
        public const string AGENT_FILE_SANITY_CHECK = "AGENT_FILE_SANITY_CHECK";
        public const string AGENT_MASTER_DATA_UPDATE = "AGENT_MASTER_DATA_UPDATE";
        public const string UPDATE_AGENT_BANKING_DATA = "UPDATE_AGENT_BANKING_DATA";
        public const string AGENT_BANKING_INFO_CTRL = "AGENT_BANKING_INFO_CTRL";
        public const string UPDATE_AIRLINE_BANKING_DATA = "UPDATE_AIRLINE_BANKING_DATA";
        public const string AIRLINE_BANKING_INFO_CTRL = "AIRLINE_BANKING_INFO_CTRL";
        public const string UPDATE_OTHER_BANKING_DATA = "UPDATE_OTHER_BANKING_DATA";
        public const string OTHER_BANKING_INFO_CTRL = "OTHER_BANKING_INFO_CTRL";

        //Operation level activity codes
        public const string AUTO_MATCHING = "AUTO_MATCHING";
        public const string MATCHING_REVIEW = "MATCHING_REVIEW";
        public const string AUTO_SOFT_MATCHING = "AUTO_SOFT_MATCHING";
        public const string SOFT_MATCHING_REVIEW = "SOFT_MATCHING_REVIEW";
        public const string CLIENT_BALANCE_CONTROL = "CLIENT_BALANCE_CONTROL";

        //Period level activity codes
        public const string SETTLEMENT_RS_VAL_CB_STATUS = "SETTLEMENT_RS_VAL_CB_STATUS";
        public const string SETTLEMENT_RS_CREATE = "SETTLEMENT_RS_CREATE";
        public const string SETTLEMENT_RS_GEN_AND_REVIEW = "SETTLEMENT_RS_GEN_AND_REVIEW";
        public const string SETTLEMENT_RS_GEN_SET_CHCKLIST = "SETTLEMENT_RS_GEN_SET_CHCKLIST";
        public const string SETTLEMENT_RS_CTRL_SET_REP_MAN = "SETTLEMENT_RS_CTRL_SET_REP_MAN";
        public const string SETTLEMENT_RS_CTRL_SET_SIG1 = "SETTLEMENT_RS_CTRL_SET_SIG1";
        public const string SETTLEMENT_RS_CTRL_SET_SIG2 = "SETTLEMENT_RS_CTRL_SET_SIG2";
        public const string SETTLEMENT_RS_PREP_PAY_FILE = "SETTLEMENT_RS_PREP_PAY_FILE";
        public const string SETTLEMENT_RS_SEND_2_SWIFT = "SETTLEMENT_RS_SEND_2_SWIFT";
        public const string SETTLEMENT_RS_ICCS_REP = "SETTLEMENT_RS_ICCS_REP";
        public const string SETTLEMENT_RS_SEND_CONFIRMED = "SETTLEMENT_RS_SEND_CONFIRMED"; 

        public const string SETTLEMENT_FR_VAL_CB_STATUS = "SETTLEMENT_FR_VAL_CB_STATUS";
        public const string SETTLEMENT_FR_CREATE = "SETTLEMENT_FR_CREATE";
        public const string SETTLEMENT_FR_GEN_AND_REVIEW = "SETTLEMENT_FR_GEN_AND_REVIEW";
        public const string SETTLEMENT_FR_GEN_SET_CHCKLIST = "SETTLEMENT_FR_GEN_SET_CHCKLIST";
        public const string SETTLEMENT_FR_CTRL_SET_REP_MAN = "SETTLEMENT_FR_CTRL_SET_REP_MAN";
        public const string SETTLEMENT_FR_CTRL_SET_SIG1 = "SETTLEMENT_FR_CTRL_SET_SIG1";
        public const string SETTLEMENT_FR_CTRL_SET_SIG2 = "SETTLEMENT_FR_CTRL_SET_SIG2";
        public const string SETTLEMENT_FR_PREP_PAY_FILE = "SETTLEMENT_FR_PREP_PAY_FILE";
        public const string SETTLEMENT_FR_SEND_XML_BANK = "SETTLEMENT_FR_SEND_XML_BANK";
        public const string SETTLEMENT_FR_ICCS_REP = "SETTLEMENT_FR_ICCS_REP";
        public const string SETTLEMENT_FR_SEND_CONFIRMED = "SETTLEMENT_FR_SEND_CONFIRMED"; 

        public const string SETTLEMENT_NR_VAL_CB_STATUS = "SETTLEMENT_NR_VAL_CB_STATUS";
        public const string SETTLEMENT_NR_CREATE = "SETTLEMENT_NR_CREATE";
        public const string SETTLEMENT_NR_GEN_AND_REVIEW = "SETTLEMENT_NR_GEN_AND_REVIEW";
        public const string SETTLEMENT_NR_GEN_SET_CHCKLIST = "SETTLEMENT_NR_GEN_SET_CHCKLIST";
        public const string SETTLEMENT_NR_CTRL_SET_REP_MAN = "SETTLEMENT_NR_CTRL_SET_REP_MAN";
        public const string SETTLEMENT_NR_CTRL_SET_SIG1 = "SETTLEMENT_NR_CTRL_SET_SIG1";
        public const string SETTLEMENT_NR_CTRL_SET_SIG2 = "SETTLEMENT_NR_CTRL_SET_SIG2";
        public const string SETTLEMENT_NR_PREP_PAY_FILE = "SETTLEMENT_NR_PREP_PAY_FILE";
        public const string SETTLEMENT_NR_SEND_XML_BANK = "SETTLEMENT_NR_SEND_XML_BANK";
        public const string SETTLEMENT_NR_SEND_CONFIRMED = "SETTLEMENT_NR_SEND_CONFIRMED"; 

        public const string BILLING_FILE_RETRIEVAL = "BILLING_FILE_RETRIEVAL";
        public const string INBOUND_SANITY_CHECK = "INBOUND_SANITY_CHECK";
        public const string BILLING_FILE_REVIEW = "BILLING_FILE_REVIEW";
        public const string CLIENT_BALANCE_UPDATE = "CLIENT_BALANCE_UPDATE";
        public const string MASTER_DATA_SNAPSHOT = "MASTER_DATA_SNAPSHOT";
        public const string REMITTANCE_SANITY_CHECK = "REMITTANCE_SANITY_CHECK";
        public const string BANK_STATEMENT_RECEPTION = "BANK_STATEMENT_RECEPTION";
        public const string BANK_STATEMENT_SANITY_CHECK = "BANK_STATEMENT_SANITY_CHECK";
        public const string BANK_STATEMENT_UPD_HINGE_BAL = "BANK_STATEMENT_UPD_HINGE_BAL";
        public const string BANK_STATEMENT_CONTROL = "BANK_STATEMENT_CONTROL";
        public const string BANK_STATEMENT_INS_MOVEMENTS = "BANK_STATEMENT_INS_MOVEMENTS";

        public const string SEND_BILLING_NOTIFICATION = "SEND_BILLING_NOTIFICATION";

        public const string HAR_VAL_CB_STATUS = "HAR_VAL_CB_STATUS";
        public const string CREATE_HAR = "CREATE_HAR";
        public const string GENERATE_REVIEW_HAR = "GENERATE_REVIEW_HAR";
        public const string INSERT_TOPMAN_SCREEN = "INSERT_TOPMAN_SCREEN";
        public const string HAR_CONTROL = "HAR_CONTROL";

        public const string ADJUSTMENT_GENERATION = "ADJUSTMENT_GENERATION";
        public const string ADJUSTMENT_FILES_CONTROL = "ADJUSTMENT_FILES_CONTROL";
        public const string CREATE_ADJUSTMENT_FILES = "CREATE_ADJUSTMENT_FILES";
        public const string SEND_ADJUSTMENT_FILES = "SEND_ADJUSTMENT_FILES";
        public const string ADJUSTMENT_SEND_CONFIRMATION = "ADJUSTMENT_SEND_CONFIRMATION";
        public const string ADJUSTMENT_UPDATE_CB = "ADJUSTMENT_UPDATE_CB";
        public const string ADJUSTMENT_REVIEW = "ADJUSTMENT_REVIEW";

        //ICCS
        public const string PRODUCE_ICCS_PRELIMINARY_RPRT = "PRODUCE_ICCS_PRELIMINARY_RPRT";
        public const string REPORT_TO_OUTBOUND = "REPORT_TO_OUTBOUND";

        public const string PRODUCE_ICCS_FINAL_RPRT = "PRODUCE_ICCS_FINAL_RPRT";
        public const string FINAL_REPORT_TO_OUTBOUND = "FINAL_REPORT_TO_OUTBOUND";
        public const string SEND_CONFIRMED_FINAL = "SEND_CONFIRMED_FINAL";
        public const string SEND_CONFIRMED_PRELIMINARY = "SEND_CONFIRMED_PRELIMINARY";

        //ShortNonPayment
        public const string SHORT_NON_PAYMENT_VALIDATE_CB_STATUS = "VALIDATE_CLIENT_BALANCES";
        public const string EXTRACT_SHORT_NON_PAYMENTS = "EXTRACT_SHORT/NON_PAYMENTS";
        public const string SHORT_NON_PAYMENT_REVIEW = "SHORT/NON_PAYMENT_REVIEW";
        public const string SHORT_NON_PAYMENT_CONTROL = "SHORT/NON_PAYMENT_CONTROL";
        public const string CREATE_SIDRA = "CREATE_SIDRA";
        public const string SEND_SIDRA = "SEND_SIDRA";

        public const string REVIEW_NEGATIVE_REMITTANCE = "REVIEW_NEGATIVE_REMITTANCE";

        //ManualPaymentManagement
        public const string MANUAL_PAYMENT_VALIDATE_CB_STATUS = "VALIDATE_CB_STATUS";
        public const string MANUAL_PAYMENT_CREATE_MANUAL_PAYMENT = "CREATE_MANUAL_PAYMENT";
        public const string MANUAL_PAYMENT_ENTER_PAYMENT_DETAILS = "ENTER_PAYMENT_DETAILS";
        public const string MANUAL_PAYMENT_COUNTRY_MANAGER_APPROVAL = "COUNTRY_MANAGER_APPROVAL";
        public const string MANUAL_PAYMENT_SIGNATORY_APPROVAL = "SIGNATORY_APPROVAL";
        public const string MANUAL_PAYMENT_SIGNATORY_APPROVAL2 = "SIGNATORY_APPROVAL2";
        public const string MANUAL_PAYMENT_CREATE_PAYMENT_FILE = "CREATE_PAYMENT_FILE";
        public const string MANUAL_PAYMENT_SEND_PAYMENT_FILE = "SEND_PAYMENT_FILE";
        public const string MANUAL_PAYMENT_SEND_CONFIRMED = "MANUAL_PAYMENT_SEND_CONFIRMED";

        //Prorations
        public const string PRORATION_VALIDATE_CB = "PRORATION_VALIDATE_CB";
        public const string CREATE_AGENT_PRORATIONS = "CREATE_AGENT_PRORATIONS";
        public const string AGENT_PRORATION_MNGT = "AGENT_PRORATION_MNGT";
        public const string CALCULATE_AGENT_PRORATION = "CALCULATE_AGENT_PRORATION";
        public const string AGENT_PRORATION_REVIEW = "AGENT_PRORATION_REVIEW";
        public const string PRORATION_CONTROL = "PRORATION_CONTROL";
        public const string PRORATION_SAVE_PLANS = "PRORATION_SAVE_PLANS";
        public const string CREATE_AIRLINE_PRORATIONS = "CREATE_AIRLINE_PRORATIONS";
        public const string AIRLINE_PRORATION_MNGT = "AIRLINE_PRORATION_MNGT";
        public const string CALCULATE_AIRLINE_PRORATION = "CALCULATE_AIRLINE_PRORATION";
        public const string AIRLINE_PRORATION_REVIEW = "AIRLINE_PRORATION_REVIEW";

        //Post Clawbacks
        public const string POST_CLAWBACKS_REVIEW_CLAWBACKS = "REVIEW_CLAWBACKS";
        public const string POST_CLAWBACKS_CLAWBACK_CONTROL = "CLAWBACK_CONTROL";
        public const string POST_CLAWBACKS_UPDATE_CLIENTBALANCES = "UPDATE_CLIENTBALANCES";

        //Remittance Discrepancy
        public const string REMITTANCE_DISCREPANCY_GENERATE_DISCREPANCY_REPORTS = "GENERATE_DISCREPANCY_REPORTS";
        public const string REMITTANCE_DISCREPANCY_CONTROL_DISCREPANCY_REPORTS = "CONTROL_DISCREPANCY_REPORTS";
        public const string REMITTANCE_DISCREPANCY_SEND_DISCREPANCY_REPORTS = "SEND_DISCREPANCY_REPORTS";
        public const string REMITTANCE_DISCREPANCY_CONFIRM_SEND_REPORTS = "CONFIRM_SEND_REPORTS";

        // Billing Adjustments
        public const string BILLING_ADJUSTMENTS_BILLING_ADJUSTMENTS_ENTRY = "BILLING_ADJUSTMENTS_ENTRY";
        public const string BILLING_ADJUSTMENTS_BILLING_ADJUSTMENTS_CONTROL = "BILLING_ADJUSTMENTS_CONTROL";
        public const string BILLING_ADJUSTMENTS_CLIENT_BALANCE_UPDATE = "BILLING_ADJUSTMENTS_CLIENT_BALANCE_UPDATE";
    }


    #endregion

    #region " Common Functions "

    public class CommonFunctions
    {
        public static DateTime TrimTimeFromDateTime(DateTime fullDateTime)
        {
            return new DateTime(fullDateTime.Year, fullDateTime.Month, fullDateTime.Day, 0, 0, 0);
        }

        /// <summary>
        /// Remove leading and trailing white spaces of each value contained in a dataset.
        /// </summary>
        /// <param name="dsData"></param>
        public static void TrimDataSetLeftAndRight(DataSet dsData)
        {

            foreach (DataTable table in dsData.Tables)
            {
                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn col in row.Table.Columns)
                    {
                        if (col.DataType == typeof(string) && row[col] != DBNull.Value)
                        {
                            row[col] = (object)(row[col].ToString().TrimStart());
                            row[col] = (object)(row[col].ToString().TrimEnd());
                        }
                    }

                }

            }
        }

        /*
        public static void ThrowDBExceptions(long errorID)
        {
            FrameworkException exFrameworkException = null;
            switch (errorID)
            {
                case 0:
                    // Do nothing
                    break;
                case 7004:
                    // Throw concurrency error
                    exFrameworkException = new FrameworkException(FrameworkExceptions.ConcurrencyError);
                    break;
                case 7009:
                    exFrameworkException = new FrameworkException(FrameworkExceptions.ResultSetTooBigRefineCriteria);
                    break;

            }

            //If we have a framework exception raised
            if (exFrameworkException != null)
            {
                string strErrorMsg = exFrameworkException.Message;

                if (strStoredProcName != "")
                {
                    strErrorMsg += " - " + strStoredProcName + "\n";
                }

                if (dictOptParams != null)
                {
                    foreach (KeyValuePair<string, object> objPair in dictOptParams)
                    {
                        strErrorMsg += String.Format("Parameter {0}: {1}\n", objPair.Key, objPair.Value.ToString());
                    }
                }
                throw new ApplicationException(strErrorMsg);
            }

        }
        */

        public static void ThrowDBExceptions(long errorID, string strStoredProcName = "", Dictionary<string, object> dictOptParams = null)
        {
            FrameworkException exFrameworkException = null;
            switch (errorID)
            {
                case 0:
                    // Do nothing
                    break;
                case 7004:
                    // Throw concurrency error
                    exFrameworkException = new FrameworkException(FrameworkExceptions.ConcurrencyError);
                    break;
                case 7009:
                    exFrameworkException = new FrameworkException(FrameworkExceptions.ResultSetTooBigRefineCriteria);
                    break;
           
            }

            //If we have a framework exception raised
            if (exFrameworkException != null)
            {
                string strErrorMsg = exFrameworkException.Message;

                if (strStoredProcName != "")
                {
                    strErrorMsg += " - " + strStoredProcName + "\n";
                }

                if (dictOptParams != null)
                {
                    foreach (KeyValuePair<string, object> objPair in dictOptParams)
                    {
                        strErrorMsg += String.Format("Parameter {0}: {1}\n", objPair.Key, objPair.Value.ToString());
                    }
                }
                throw new ApplicationException(strErrorMsg);
            }
          
        }

      
        /*
        public static void RethrowDBExceptions(string message)
        {
            long lngErrorNumber = 0;
            if (long.TryParse(message, out lngErrorNumber))
            {
                switch (lngErrorNumber)
                {
                    case 0:
                        // Do nothing
                        break;
                    case 7004:
                        // Throw concurrency error
                        throw new FrameworkException(FrameworkExceptions.ConcurrencyError);
                    case 7009:
                        throw new FrameworkException(FrameworkExceptions.ResultSetTooBigRefineCriteria);
                }
            }
        }
        */

        public static void InsertEventToSystemLog(string strSource, string strMessage)
        {
            if (!System.Diagnostics.EventLog.SourceExists(strSource))
            {
                EventLog.CreateEventSource(strSource, "Application");
                EventLog.WriteEntry(strSource, strMessage);

            }
            else
            {
                EventLog.WriteEntry(strSource, strMessage);

            }

        }

        public static bool ValidateBizTalkWebCallUsername(string strUsername)
        {
            try
            {
                string strExpectedUsername = CommonFunctions.GetConfigFileValue("Usernames/BizTalkUser");

                if (strUsername == strExpectedUsername)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }



        }

        public static bool ValidateBizTalkWebCallIP(string strIP)
        {
            try
            {
                string strExpectedIP = CommonFunctions.GetConfigFileValue("IPs/BizTalkIP");

                //if (strExpectedIP.Contains(strIP))
                //if (strExpectedIP.ToLower().Contains(Dns.GetHostEntry(strIP).HostName.ToString().ToLower()))
                if (strExpectedIP.ToLower().Contains(Dns.GetHostEntry(strIP).HostName.ToString().ToLower()) ||
                        strExpectedIP.Contains(strIP))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public static bool ValidateLocalhostWebCallIP(string strIP)
        {
            try
            {
                string strExpectedIP = CommonFunctions.GetConfigFileValue("IPs/LocalhostIPs");

                if (strExpectedIP.Contains(strIP))
                    return true;
                else
                {
                    strExpectedIP = CommonFunctions.GetConfigFileValue("IPs/LocalhostIPs");

                    if (strExpectedIP.Contains(strIP))
                        return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        public static bool ValidateWebCallIP(string strIP = "")
        {
            try
            {
                if (strIP == "")
                {
                    OperationContext context = OperationContext.Current;
                    MessageProperties prop = context.IncomingMessageProperties;
                    RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                    strIP = endpoint.Address;
                }

                return ValidateBizTalkWebCallIP(strIP) || ValidateLocalhostWebCallIP(strIP);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static int CountStringOccurences(string strNeedle, string strHaystack)
        {
            return (strHaystack.Length - strHaystack.Replace(strNeedle, string.Empty).Length) / strNeedle.Length;
        }

        public static void ConfigureReportingServiceReportViewer(Microsoft.Reporting.WebForms.ReportViewer rptvReportViewer, System.Collections.Specialized.NameValueCollection colReportParameters)
        {
            string strReportName = null;
            bool blnIsForms = false;

            ReportParameterInfoCollection objReportParameterInfoColl = default(ReportParameterInfoCollection);
            List<Microsoft.Reporting.WebForms.ReportParameter> objReportParameterList = new List<Microsoft.Reporting.WebForms.ReportParameter>();

            int i = 0;

            try
            {
                strReportName = colReportParameters["ReportName"];

                rptvReportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                rptvReportViewer.ServerReport.ReportServerUrl = new Uri(CommonFunctions.GetConfigFileValue("WebService[@name='ReportingService']/Location/@url"));

                blnIsForms = (CommonFunctions.GetConfigFileValue("WebService[@name='ReportingService']/@IsFormsAuthentication", "0") == "1");

                if (blnIsForms)
                {
                    rptvReportViewer.ServerReport.ReportServerCredentials = new ReportServerFormsCredentials();
                }
                else
                {
                    rptvReportViewer.ServerReport.ReportServerCredentials = new ReportServerNetworkCredentials();
                }
                
                rptvReportViewer.ServerReport.Timeout = System.Threading.Timeout.Infinite;
                rptvReportViewer.ServerReport.ReportPath = "/" + CommonFunctions.GetConfigFileValue("WebService[@name='ReportingService']/Folder") + "/" + strReportName;
                
                objReportParameterInfoColl = rptvReportViewer.ServerReport.GetParameters();
                // ERROR: Not supported in C#: ReDimStatement


                for (i = 0; i <= objReportParameterInfoColl.Count - 1; i++)
                {
                    if ((objReportParameterInfoColl[i].Name == "UserName" || objReportParameterInfoColl[i].Name == "Username") && !objReportParameterInfoColl[i].Visible)
                    {
                        objReportParameterList.Insert(i, new Microsoft.Reporting.WebForms.ReportParameter(objReportParameterInfoColl[i].Name, GetUserName()));
                    }
                    else if (objReportParameterInfoColl[i].Name == "LanguageCode")
                    {
                        if ((colReportParameters[objReportParameterInfoColl[i].Name] != null))
                        {
                            objReportParameterList.Insert(i, new Microsoft.Reporting.WebForms.ReportParameter(objReportParameterInfoColl[i].Name, azNullIDToString(colReportParameters[objReportParameterInfoColl[i].Name])));
                        }
                        else
                        {
                            if ((colReportParameters[objReportParameterInfoColl[i].Name] != null))
                            {
                                objReportParameterList.Insert(i, new Microsoft.Reporting.WebForms.ReportParameter(objReportParameterInfoColl[i].Name, azNullIDToString(colReportParameters[objReportParameterInfoColl[i].Name])));
                            }
                            else
                            {
                                objReportParameterList.Insert(i, new Microsoft.Reporting.WebForms.ReportParameter(objReportParameterInfoColl[i].Name, GetUserLanguage()));
                            }
                        }
                    }
                    else
                    {
                        if ((objReportParameterInfoColl[i].DataType == ParameterDataType.Boolean))
                        {
                            if ((azNullIDToNothing(colReportParameters[objReportParameterInfoColl[i].Name]) == null))
                            {
                                objReportParameterList.Insert(i, new Microsoft.Reporting.WebForms.ReportParameter(objReportParameterInfoColl[i].Name, azNullIDToString(colReportParameters[objReportParameterInfoColl[i].Name])));
                            }
                            else
                            {
                                objReportParameterList.Insert(i, new Microsoft.Reporting.WebForms.ReportParameter(objReportParameterInfoColl[i].Name, azNullIDToString(colReportParameters[objReportParameterInfoColl[i].Name])));
                            }
                        }
                        else
                        {
                            objReportParameterList.Insert(i, new Microsoft.Reporting.WebForms.ReportParameter(objReportParameterInfoColl[i].Name, azNullIDToString(colReportParameters[objReportParameterInfoColl[i].Name])));
                        }
                    }
                }

                if (objReportParameterList != null && objReportParameterList.Count > 0)
                    rptvReportViewer.ServerReport.SetParameters(objReportParameterList);

                //ConfigureReportingServiceRights();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void LogEvent(string strEventText)
        {
            LogEntry objLogEntry = new LogEntry();
            objLogEntry.Message = strEventText;
            Logger.Write(objLogEntry);
        }

        public static void ConfigureReportingServiceRights()
        {
            ReportingService2010Forms rsWebService;
            bool inherit;

            try
            {
                rsWebService = new ReportingService2010Forms();
                rsWebService.Url = CommonFunctions.GetConfigFileValue("WebService[@name='ReportingService']/Location/@url");
                rsWebService.Credentials = new ReportServer2010Credentials();

                Policy[] policies = rsWebService.GetPolicies("DataSources", out inherit);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static object azNullIDToNothing(string strValue)
        {

            try
            {
                if (strValue == Convert.ToString(azConstants.NullID) || strValue == "null")
                {
                    return null;
                }
                else
                {
                    return strValue;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string azNullIDToString(string strValue)
        {

            try
            {
                if (strValue == Convert.ToString(azConstants.NullID) || strValue == "null")
                {
                    return null;
                }
                else
                {
                    return strValue;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicParameters"></param>
        /// <returns></returns>
        public static Array CreateArrayFromDictionary(Dictionary<string, object> dicParameters)
        {
            Array arrResults = null;

            if (dicParameters == null)
                return ArrayLibrary.CreateArray();

            foreach (object objValue in dicParameters.Values)
            {
                object objToAdd = objValue.ToString();

                if (objValue == DBNull.Value)
                    objToAdd = DBNull.Value;

                if (arrResults == null)
                {
                    arrResults = ArrayLibrary.CreateArray(objToAdd);
                }
                else
                {
                    arrResults = ArrayLibrary.AppendItemToArray(arrResults, objToAdd);
                }
            }

            return arrResults;
        }

        public static void DataTablesToXML(string strFileName, DataTable d1, DataTable d2)
        {
            DataSet dsNewDS1 = d1.DataSet;
            DataSet dsNewDS2 = d2.DataSet;
            if (dsNewDS1 == null)
            {
                dsNewDS1 = new DataSet();
                dsNewDS1.Tables.Add(d1);
            }
            if (dsNewDS2 == null)
            {
                dsNewDS2 = new DataSet();
                dsNewDS2.Tables.Add(d2);
            }

            string xmlDS1 = XMLLibrary.Dataset2XML(dsNewDS1);
            string xmlDS2 = XMLLibrary.Dataset2XML(dsNewDS2);

            xmlDS1 = xmlDS1.Replace("&#x16;", "");
            xmlDS2 = xmlDS2.Replace("&#x16;", "");

            xmlDS1 = xmlDS1.Replace("Tablas", "Table");
            xmlDS2 = xmlDS2.Replace("Tablas", "Table");

            xmlDS1 = xmlDS1.Replace("<Name>", "<NAME>");
            xmlDS1 = xmlDS1.Replace("</Name>", "</NAME>");

            xmlDS1 = xmlDS1.Replace("<Solution>", "<NewDataSet>");
            xmlDS1 = xmlDS1.Replace("</Solution>", "</NewDataSet>");

            xmlDS1 = xmlDS1.Replace("<Date>", "<DATE>");
            xmlDS1 = xmlDS1.Replace("</Date>", "</DATE>");

            XDocument xDoc1 = XDocument.Parse(xmlDS1);
            XDocument xDoc2 = XDocument.Parse(xmlDS2);

            xDoc1.Save("C:/temp/SqlExtraction/" + strFileName + "-1.xml");
            xDoc2.Save("C:/temp/SqlExtraction/" + strFileName + "-2.xml");
        }

        /// <summary>
        /// To avoid DateTime comparison faults in the database, date strings need to be formatted in the DMY format.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        [Obsolete("Use DateTimeLibrary class instead")]
        public static string ConvertToDMYDateFormat(string date)
        {
            DateTime dateObject = DateTime.Parse(date, CultureInfo.CurrentCulture);
            return dateObject.ToString("dd/MM/yyyy hh:mm:ss tt");
        }

        /// <summary>
        /// Convert source time to hub local time
        /// </summary>
        /// <param name="dtSourceTime"></param>
        /// <returns></returns>
        public static DateTime ConvertToHUBTime(DateTime dtSourceTime)
        {
            try
            {
                string timeZoneID = CommonFunctions.GetConfigFileValue("TimeZone/@Name");
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZoneID);
                return TimeZoneInfo.ConvertTime(dtSourceTime, timeZoneInfo);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string GenerateJSConstantsFromClassConstants(string strNameSpace, string strClassName)
        {

            string strResult;

            try
            {

                strResult = " // Constants for Class " + strNameSpace + "." + strClassName + '\n';
                strResult = strResult + "var " + strClassName + " = new Object();" + '\n';

                foreach (FieldInfo objField in Type.GetType(strNameSpace + "." + strClassName).GetFields())
                {

                    strResult = strResult + '\n' + strClassName + "." + objField.Name + " = \"" + objField.GetValue(null) + "\";";

                }

                return strResult + '\n' + '\n';
            }

            catch
            {
                throw;
            }

        }

        /// <summary>
        ///
        /// </summary>
        public static string GenerateJSConstantsFromEnum(string strNameSpace, string strEnumName)
        {

            string strResult;
            int i;

            try
            {

                strResult = " // Constants for Enum " + strNameSpace + "." + strEnumName + '\n';
                strResult = strResult + "var i = 0;" + '\n';
                strResult = strResult + "var " + strEnumName + " = new Object();" + '\n';

                for (i = 0; i <= Enum.GetNames(Type.GetType(strNameSpace + "." + strEnumName)).Length - 1; i++)
                {

                    strResult = strResult + '\n' + strEnumName + "." + Enum.GetNames(Type.GetType(strNameSpace + "." + strEnumName))[i] + " = i++;";
                    //Type.GetType(strEnumName))(i)

                }

                return strResult + '\n' + '\n';
            }

            catch
            {
                throw;
            }

        }

        public static string GetSystemTempFolder()
        {
            return System.Environment.GetEnvironmentVariable("TEMP");
        }


        /// <summary>
        /// Returns the current username
        /// </summary>
        public static string GetUserName()
        {

            try
            {
                if (HttpContext.Current != null)
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                    {
                        if (System.Web.HttpContext.Current.Items["Username"] != null)
                        {
                            return System.Web.HttpContext.Current.Items["Username"].ToString();
                        }
                        else
                        {
                            if (!IsSuperUser())
                            {
                                return GetSuperUserName();
                            }
                        }

                    }

                    return HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    if (!IsSuperUser())
                    {
                        return GetSuperUserName();
                    }
                }

                return GetSuperUserName();
            }
            catch
            {
                throw;
            }
        }

        public static string GetSuperUserName()
        {
            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint.Address;

            if (CommonFunctions.ValidateBizTalkWebCallIP(ip))
            {
                return CommonFunctions.GetConfigFileValue("Usernames/BizTalkUser");
            }
            else if (CommonFunctions.ValidateLocalhostWebCallIP(ip))
            {
                return CommonFunctions.GetConfigFileValue("Usernames/Localhost");
            }

            return "";
        }

        public static bool IsSuperUser()
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    if (string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                    {
                        if (System.Web.HttpContext.Current.Items["Username"] != null)
                        {
                            return System.Web.HttpContext.Current.Items["Username"].ToString() == CommonFunctions.GetConfigFileValue("Usernames/SchedulerUser").ToString();
                        }
                        else
                        {
                            return CommonFunctions.ValidateWebCallIP();
                        }

                    }

                    return HttpContext.Current.User.Identity.Name == CommonFunctions.GetConfigFileValue("Usernames/SchedulerUser").ToString();
                }
                else
                {
                    return CommonFunctions.ValidateWebCallIP();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string GetUserLanguage()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpper(); ;
        }

        /// <summary>
        ///
        /// </summary>
        public static string GetCurrentCulture()
        {
            return System.Threading.Thread.CurrentThread.CurrentUICulture.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        public static string GetWSValue(string webServiceName, string configFileWSInfo)
        {
            ApplicationConfiguration objApplicationConfiguration;

            try
            {
                objApplicationConfiguration = new ApplicationConfiguration();
                objApplicationConfiguration.Init(azConstants.BusinessTaskID);

                return objApplicationConfiguration.GetConfigValue("WebService[@name='" + webServiceName + "']" + configFileWSInfo, "");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string GetConfigFileValue(string strBusinessTaskID, string strXPath, string strDefaultvalue = null)
        {
            ApplicationConfiguration objApplicationConfiguration;

            try
            {
                objApplicationConfiguration = new ApplicationConfiguration();
                objApplicationConfiguration.Init(strBusinessTaskID);

                return objApplicationConfiguration.GetConfigValue(strXPath, strDefaultvalue);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string GetConfigFileValue(string strXPath, string strDefaultvalue = null)
        {
            ApplicationConfiguration objApplicationConfiguration;

            try
            {
                objApplicationConfiguration = new ApplicationConfiguration();
                objApplicationConfiguration.Init(azConstants.BusinessTaskID);

                return objApplicationConfiguration.GetConfigValue(strXPath, strDefaultvalue);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static long GetMaxFindResults(ClassID lngClassID)
        {

            try
            {
                return long.Parse(CommonFunctions.GetConfigFileValue("MaxResults/Application[@ClassID='" + (long)lngClassID + "']/Find", "100"));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static long GetMaxReadForEntityRelationResults(ClassID lngClassID)
        {

            try
            {
                return long.Parse(CommonFunctions.GetConfigFileValue("MaxResults/Application[@ClassID='" + (long)lngClassID + "']/ReadForEntityRelation", "100"));
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Static method to return an instance of CustomBinding
        /// </summary>
        /// <returns></returns>
        public static CustomBinding GetNewCustomBinding(bool isHttps = false)
        {
            BinaryMessageEncodingBindingElement objBinaryMessageEncodingBinding = new BinaryMessageEncodingBindingElement();
            objBinaryMessageEncodingBinding.ReaderQuotas.MaxStringContentLength = 2147483647;
            objBinaryMessageEncodingBinding.ReaderQuotas.MaxArrayLength = 2147483647;
            objBinaryMessageEncodingBinding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            if (isHttps)
            {
                HttpsTransportBindingElement httpTransport = new HttpsTransportBindingElement();
                httpTransport.MaxReceivedMessageSize = 2147483647;
                httpTransport.MaxBufferSize = 2147483647;

                return new CustomBinding(objBinaryMessageEncodingBinding, httpTransport);
            }
            else
            {
                HttpTransportBindingElement httpTransport = new HttpTransportBindingElement();
                httpTransport.MaxReceivedMessageSize = 2147483647;
                httpTransport.MaxBufferSize = 2147483647;

                return new CustomBinding(objBinaryMessageEncodingBinding, httpTransport);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static DataSet DataTableToDataSet(DataTable dv, string strTableName)
        {

            DataTable dtTemp = null;
            DataRow drv = null;
            DataSet dsTemp = null;

            try
            {

                //// Copy the table
                dtTemp = dv.Clone();

                //// Clone the structure of the table behind the view
                dtTemp.TableName = strTableName;

                //// Populate the table with rows in the view
                foreach (DataRow drv_loopVariable in dv.Rows)
                {
                    drv = drv_loopVariable;
                    dtTemp.ImportRow(drv);
                }

                dsTemp = new DataSet(dv.TableName);

                //// Add the new table to a DataSet
                dsTemp.Tables.Add(dtTemp);

                return dsTemp;

            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        ///
        /// </summary>
        public static DataSet CopyDataRowToNewDataSet(DataRow drRow)
        {

            DataTable dtTemp = null;
            DataSet dsTemp = null;

            try
            {
                dtTemp = drRow.Table.Clone();

                dtTemp.ImportRow(drRow);

                dsTemp = new DataSet(dtTemp.TableName);

                //// Add the new table to a DataSet
                dsTemp.Tables.Add(dtTemp);

                return dsTemp;

            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public static DataSet FilterDataTableAsNewDataset(DataTable dtTable, string strFilter, string strSort = "")
        {

            DataView dvDataView = null;


            try
            {
                dvDataView = new DataView(dtTable);
                dvDataView.RowFilter = strFilter;
                if (strSort != "")
                {
                    dvDataView.Sort = strSort;
                }

                return DataViewToDataSet(dvDataView, "Table");

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static DataSet DataViewToDataSet(DataView dv, string strTableName)
        {

            DataTable dtTemp = null;
            DataRowView drv = null;
            DataSet dsTemp = null;


            try
            {
                //// Copy the table
                dtTemp = dv.Table.Clone();

                //// Clone the structure of the table behind the view
                dtTemp.TableName = strTableName;

                //// Populate the table with rows in the view
                foreach (DataRowView drv_loopVariable in dv)
                {
                    drv = drv_loopVariable;
                    dtTemp.ImportRow(drv.Row);
                }

                dsTemp = new DataSet(dv.Table.TableName);

                //// Add the new table to a DataSet
                dsTemp.Tables.Add(dtTemp);

                return dsTemp;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        ///
        /// </summary>
        public static byte[] GetTempDocumentContent(string strGUID, bool blnDeleteAfterRead)
        {

            FileStream objFileStream = null;
            BinaryReader objBinaryReader = null;

            byte[] bytArray = null;

            try
            {

                objFileStream = new FileStream(GetTempDocumentFileName(strGUID), FileMode.Open, FileAccess.Read);

                objBinaryReader = new BinaryReader(objFileStream);
                bytArray = objBinaryReader.ReadBytes((int)objFileStream.Length);

                objBinaryReader.Close();
                objFileStream.Close();

                if (blnDeleteAfterRead)
                {
                    try
                    {
                        File.Delete(GetTempDocumentFileName(strGUID));
                    }
                    catch (Exception)
                    {

                    }
                }

                return bytArray;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objBinaryReader.Close();
                objFileStream.Close();

            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string GetTempDocumentFileName(string strGUID)
        {
            try
            {

                return Path.Combine(System.Environment.GetEnvironmentVariable("TEMP"), strGUID);

            }
            catch (Exception)
            {
                throw;

            }
        }

        /// <summary>
        ///
        /// </summary>
        public static byte[] GetThumbNailImage(byte[] arrByteLogo, int intWidth, int intHeight)
        {

            Image imgTemp = default(System.Drawing.Image);
            System.IO.MemoryStream objMs = default(System.IO.MemoryStream);

            //Convert The Byte Array in image
            objMs = new System.IO.MemoryStream(arrByteLogo, 0, arrByteLogo.Length);
            objMs.Write(arrByteLogo, 0, arrByteLogo.Length);
            imgTemp = System.Drawing.Image.FromStream(objMs, true);

            // Get the ThumbNailImage
            imgTemp = ScaleImageBySize(imgTemp, intWidth, intHeight);

            //Convert the ThuimbNail Image in Byte array
            objMs = new System.IO.MemoryStream(imgTemp.Size.Width * imgTemp.Size.Height);

            imgTemp.Save(objMs, System.Drawing.Imaging.ImageFormat.Png);

            return objMs.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        public static byte[] GetEmptyImage()
        {

            Stream ms = null;
            BinaryReader objBinaryReader = null;

            byte[] bytArray = null;

            try
            {
                ms = LoadResource("Blank.png");
                objBinaryReader = new BinaryReader(ms);
                bytArray = objBinaryReader.ReadBytes((int)ms.Length);

                return bytArray;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                objBinaryReader.Close();
                ms.Close();

            }
        }

        /// <summary>
        ///
        /// </summary>
        public static Stream LoadResource(string strResourceName)
        {
            Stream objResource = default(Stream);

            objResource = Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().FullName.Split(',')[0] + "." + strResourceName);

            if (objResource == null)
            {
                throw new Exception("Resource " + strResourceName + " not found.");
            }

            return objResource;

        }

        /// <summary>
        ///
        /// </summary>
        public static System.Drawing.Image ScaleImageBySize(System.Drawing.Image imgPhoto, int intWidth, int intHeight)
        {

            int imgWidth = 0;
            int imgHeight = 0;
            int newImageWidth = 0;
            int newImageHeight = 0;
            int X = 0;
            int Y = 0;
            int DX = 0;
            int DY = 0;

            imgWidth = imgPhoto.Width;
            imgHeight = imgPhoto.Height;

            X = 0;
            Y = 0;
            DX = 0;
            DY = 0;

            float fltRapport;

            if (imgWidth > ((intWidth / intHeight) * imgHeight))
            {
                newImageWidth = intWidth;
                fltRapport = ((float)intWidth / (float)imgWidth);
                newImageHeight = (int)(fltRapport * imgHeight);
            }
            else
            {
                newImageHeight = intHeight;
                fltRapport = ((float)intHeight / (float)imgHeight);
                newImageWidth = (int)(fltRapport * imgWidth);
            }

            System.Drawing.Bitmap newImage = default(System.Drawing.Bitmap);
            newImage = new System.Drawing.Bitmap(newImageWidth, newImageHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            newImage.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);
            System.Drawing.Graphics gr = default(System.Drawing.Graphics);
            gr = System.Drawing.Graphics.FromImage(newImage);
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            gr.DrawImage(imgPhoto, new System.Drawing.Rectangle(DX, DY, newImageWidth, newImageHeight), new System.Drawing.Rectangle(X, Y, imgWidth, imgHeight), System.Drawing.GraphicsUnit.Pixel);
            gr.Dispose();

            return newImage;
        }


        public static string ImportFile(string strFileName, string strFilePath, bool bCheckExtension, string strFileExtension, string strBinaryFileString, Encoding objEncoding)
        {
            //throw new Exception("FILENAME - " + strFileName+"\r\n FILEPATH - "+ strFilePath);
            string strFullFileName;
            string strFullFileNamePath;

            // We verify the the uploaded file extension is of the correct type
            if (bCheckExtension)
            {
                if (!System.IO.Path.GetExtension(strFileName).ToLower().Equals(strFileExtension.ToLower()))
                    throw new ApplicationException("The uploaded file type is incorrect");
            }

            // We create the directory needed for the import if it is not already created
            System.IO.Directory.CreateDirectory(strFilePath);

            // We get the full file name and path
            strFullFileName = Guid.NewGuid() + "_" + System.IO.Path.GetFileName(strFileName);

            // We create the full file name and path
            strFullFileNamePath = strFilePath + "\\" + strFullFileName;

            // We create the file into which we will import the data and 
            // write the contents of the strBinaryString variable into it
            
            FileInfo fi = new FileInfo(@strFullFileNamePath);
            StreamWriter sw = new StreamWriter(fi.FullName, true, objEncoding);
            sw.Write(strBinaryFileString);
            sw.Close();

            return strFullFileName;
        }

        public static void LoadAndExecuteSSISPackage(string strSSISPackagePath, SerializableDictionary<string, object> dicParams)
        {
            Microsoft.SqlServer.Dts.Runtime.Application app;
            Package package = null;

            try
            {
                if (!IsIPAddressValid(HttpContext.Current.Request.UserHostAddress))
                    throw new IPAddressNotAllowedException(HttpContext.Current.Request.UserHostAddress);

                ImportEventListener eventListener = new ImportEventListener();

                //Load DTSX
                app = new Microsoft.SqlServer.Dts.Runtime.Application();
                package = app.LoadPackage(strSSISPackagePath, eventListener);

                //Global Package Variable
                Variables vars = package.Variables;

                // Convert XML string to Serializable dictionary
                //SerializableDictionary<string, object> dicParams = new SerializableDictionary<string, object>();
                //dicParams.LoadFromXML(strXMLDicParam);

                foreach (string param in dicParams.Keys)
                {
                    vars[param].Value = dicParams[param];
                }

                //Execute DTSX.
                Microsoft.SqlServer.Dts.Runtime.DTSExecResult results = package.Execute();

                if (results == DTSExecResult.Failure)
                {
                    string strCustomErrorMsg = "The execution of the DTSX package failed. " +
                                              "Please make sure that all the required parameters " +
                                              "are correct and the the user executing the package " +
                                              "has the approriate rights to access the DTSX package " +
                                              "and the flat files needed. \r\n";
                    string stgrErrorConcatenation = "";

                    foreach (DtsError err in package.Errors)
                    {
                        //stgrErrorConcatenation += "ErrorCode : " + err.ErrorCode + "\r\n";
                        stgrErrorConcatenation += "Error description : " + err.Description + "\r\n";
                        string test = err.Source;
                    }

                    // Create a list of the parameters and the values sent to the DTSX package
                    string strParamListAndValues = "";

                    foreach (string param in dicParams.Keys)
                    {
                        if (dicParams[param] != null)
                            strParamListAndValues += "Parameter: " + param + " Value: " + dicParams[param].ToString() + "\r\n";
                    }


                    strCustomErrorMsg += stgrErrorConcatenation;

                    strCustomErrorMsg += "\r\n" + "Parameter List ----" + "\r\n" + strParamListAndValues;

                    throw new Exception(strCustomErrorMsg);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (package != null)
                {
                    package.Dispose();
                }
                package = null;
                app = null;

            }
        }

        private static bool IsIPAddressValid(string IP_Address)
        {

            //if ((Settings.Default.AllowedIPs.IndexOf(IP_Address) == -1))
            //{
            //    return false;
            //}

            return true;
        }

        private class IPAddressNotAllowedException : ApplicationException
        {


            public IPAddressNotAllowedException(string strIP)
                : base("Your IP address: " + strIP + " is invalid for calling this web service.")
            {
            }
        }

        private static bool AreAllEmailForcedToAdmin()
        {

            string strResult = null;

            strResult = GetInfo("SystemConfig/Email/@ForceAllEmailToAdmin");

            if (string.IsNullOrEmpty(strResult))
            {
                throw new ApplicationException("Configuration item \"SystemConfig/Email/@ForceAllEmailToAdmin\" is missing from the config file");
            }

            return (strResult == "1");

        }

        public static string GetInfo(string strConfigItemName, string strDefaultValue = null)
        {

            ApplicationConfiguration objApplicationConfiguration = default(ApplicationConfiguration);

            objApplicationConfiguration = new ApplicationConfiguration();
            objApplicationConfiguration.Init(azConstants.BusinessTaskID);

            return objApplicationConfiguration.GetConfigValue(strConfigItemName, strDefaultValue);

        }

        public static void SendEmail(
          string strToEmails,
          string strSubject,
          string strBody,
          bool boolHTMLEmail = false,
          string strFromEmail = "",
          string strFromFullName = "",
          string strReplyToEmail = "",
          System.Array arrAttachmentsName = null,
          string strCCEmail = "",
          string strBCCEmail = "",
          Array embeddedImages = null)
        {
            string enabled;
            MailMessage objMail;
            string strDefaultFromEmail;
            string strSMTPHost;
            string strDefaultFromFullName = "";
            string strFROM;
            Attachment objMailAttachment;
            SmtpClient objSMTP;
            string strTmpFileToAttach;
            string strTmpFileNameinEmail;

            string strFilePath;
            string strLogicalName;

            try
            {
                enabled = GetInfo("SystemConfig/Email/@Enabled");

                if (enabled == "" || enabled == null || enabled != "1")
                    return;

                strSMTPHost = GetInfo("SystemConfig/Email/SMTPHost");

                if (strSMTPHost == "" || strSMTPHost == null)
                    throw new ApplicationException("Configuration item \"SystemConfig/Email/SMTPHost\" is missing from the config file");

                if (strFromFullName == "" || strFromFullName == null)
                {
                    strDefaultFromFullName = GetInfo("SystemConfig/Email/DefaultFromFullName");

                    if (strDefaultFromFullName == "" || strDefaultFromFullName == null)
                        throw new ApplicationException("Configuration item \"SystemConfig/Email/DefaultFromFullName\" is missing from the config file");
                }

                //Build the from Email with fullname
                if (strFromEmail != "" || strFromEmail == null)
                {
                    strFROM = "\"" + strFromFullName + "\" <" + strFromEmail + ">";
                }
                else
                {
                    // Take the Default From and Full Name
                    strDefaultFromEmail = GetInfo("SystemConfig/Email/DefaultFromEmail");

                    if (strDefaultFromEmail == "" || strDefaultFromEmail == null)
                        throw new ApplicationException("Configuration item \"SystemConfig/Email/DefaultFromEmail\" is missing from the config file");

                    strFROM = "\"" + strDefaultFromFullName + "\" <" + strDefaultFromEmail + ">";
                }

                //Check if all outgoing emails should be redirected to admin (development safety)
                if (AreAllEmailForcedToAdmin())
                {
                    // Add a special note to mention who this email was originally meant to be sent to
                    strSubject = "*Forced to admin - To : " + strToEmails + " * " + strSubject;

                    strToEmails = GetInfo("SystemConfig/Email/AdminEmail");

                    if (strToEmails == "" || strToEmails == null)
                        throw new ApplicationException("Configuration item \"SystemConfig/Email/AdminEmail\" is missing from the config file");

                    if (strCCEmail != "" || strCCEmail == null)
                    {
                        strCCEmail = GetInfo("SystemConfig/Email/AdminEmail");

                        if (strCCEmail == "" || strCCEmail == null)
                            throw new ApplicationException("Configuration item \"SystemConfig/Email/AdminEmail\" is missing from the config file");

                    }

                    if (strBCCEmail != "" || strBCCEmail == null)
                    {
                        strBCCEmail = GetInfo("SystemConfig/Email/AdminEmail");

                        if (strCCEmail == "" || strCCEmail == null)
                            throw new ApplicationException("Configuration item \"SystemConfig/Email/AdminEmail\" is missing from the config file");
                    }
                }

                if ((!strToEmails.Equals("") || strToEmails == null) && (strToEmails.Substring(strToEmails.Length - 1).Equals(";") || strToEmails.Substring(strToEmails.Length - 1).Equals(",")))
                {
                    strToEmails = strToEmails.Substring(0, strToEmails.Length - 1);
                }

                if ((!strCCEmail.Equals("") || strCCEmail == null) && (strCCEmail.Substring(strCCEmail.Length - 1).Equals(";") || strCCEmail.Substring(strCCEmail.Length - 1).Equals(",")))
                {
                    strCCEmail = strCCEmail.Substring(0, strCCEmail.Length - 1);
                }

                if ((!strBCCEmail.Equals("") || strBCCEmail == null) && (strBCCEmail.Substring(strBCCEmail.Length - 1).Equals(";") || strBCCEmail.Substring(strBCCEmail.Length - 1).Equals(",")))
                {
                    strBCCEmail = strBCCEmail.Substring(0, strBCCEmail.Length - 1);
                }


                // create a new mail message
                objMail = new MailMessage();

                objMail.IsBodyHtml = boolHTMLEmail;

                objMail.From = new MailAddress(strFROM);
                objMail.To.Add(strToEmails.Replace(";", ","));

                if (strCCEmail != "")
                    objMail.CC.Add(strCCEmail.Replace(";", ","));

                if (strBCCEmail != "")
                    objMail.Bcc.Add(strBCCEmail.Replace(";", ","));

                if (strReplyToEmail != "")
                    objMail.Headers.Add("Reply-To", strReplyToEmail);

                // We get rid of the carrige returns and other return characters
                strSubject = strSubject.Replace("\r\n", "");
                strSubject = strSubject.Replace("\r", "");
                strSubject = strSubject.Replace("\n", "");

                if (boolHTMLEmail)
                {
                    AlternateView av = AlternateView.CreateAlternateViewFromString(strBody, null, "text/html");

                    if ((embeddedImages != null) && (embeddedImages.Length > 0))
                    {
                        foreach (Array arrembeddedImage in embeddedImages)
                        {
                            strFilePath = arrembeddedImage.GetValue(0).ToString();
                            strLogicalName = arrembeddedImage.GetValue(1).ToString();

                            if (strFilePath != "" && strLogicalName != "")
                            {
                                LinkedResource logo = new LinkedResource(strFilePath);
                                logo.ContentId = strLogicalName;

                                av.LinkedResources.Add(logo);
                            }
                        }
                    }

                    objMail.AlternateViews.Add(av);
                }
                else
                {
                    objMail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(strBody, null, "text/plain"));
                }

                objMail.Subject = strSubject;

                //objMail.Body = strBody;

                if ((arrAttachmentsName != null) && (arrAttachmentsName.Length > 0))
                {
                    foreach (Array arrTmpFileToAttach in arrAttachmentsName)
                    {
                        strTmpFileNameinEmail = arrTmpFileToAttach.GetValue(0).ToString();
                        strTmpFileToAttach = arrTmpFileToAttach.GetValue(1).ToString();

                        // Attach a file if required
                        if (strTmpFileToAttach != "")
                        {
                            objMailAttachment = new Attachment(strTmpFileToAttach);
                            objMailAttachment.Name = strTmpFileNameinEmail;
                            objMail.Attachments.Add(objMailAttachment);
                        }
                    }
                }

                objSMTP = new SmtpClient();
                objSMTP.Host = strSMTPHost;

                objSMTP.Send(objMail);
            }
            catch
            {
                throw;
            }
        }
    }

    /// <summary>
    /// This class implements some functionality for managing files and directories
    /// </summary>
    static public class CommonFileFunctions
    {
        #region DeleteDirectory

        /// <summary>
        /// Delete all directory with files and subdirectories
        /// </summary>
        /// <param name="target_dir">Full path name directoty</param>
        public static void DeleteDirectory(string target_dir)
        {
            DeleteDirectoryFiles(target_dir);
            while (Directory.Exists(target_dir))
            {
                DeleteDirectoryDirs(target_dir);
            }
        }

        private static void DeleteDirectoryDirs(string target_dir)
        {
            System.Threading.Thread.Sleep(100);

            if (Directory.Exists(target_dir))
            {

                string[] dirs = Directory.GetDirectories(target_dir);

                if (dirs.Length == 0)
                    Directory.Delete(target_dir, false);
                else
                    foreach (string dir in dirs)
                        DeleteDirectoryDirs(dir);
            }
        }

        private static void DeleteDirectoryFiles(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectoryFiles(dir);
            }
        }
        #endregion

        /// <summary>
        /// Remove property readOnly for directory
        /// </summary>
        /// <param name="strFullPathName">Full path directory</param>
        public static void RemoveReadOnlyDirectory(string strFullPathName)
        {
            var di = new DirectoryInfo(strFullPathName);
            di.Attributes &= ~FileAttributes.ReadOnly;
        }

        /// <summary>
        /// Create directory if directory exist is deleted
        /// </summary>
        /// <param name="path">Full path for creation</param>
        public static void CreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                DeleteDirectory(path);

                Directory.CreateDirectory(path);
            }
            else
                Directory.CreateDirectory(path);

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);
            System.Security.AccessControl.FileSystemAccessRule fsar = new System.Security.AccessControl.FileSystemAccessRule("Users", System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.AccessControlType.Allow);
            System.Security.AccessControl.DirectorySecurity ds = null;

            ds = di.GetAccessControl();
            ds.AddAccessRule(fsar);
            di.SetAccessControl(ds);
        }
    }

    /// <summary>
    /// This class implements some functionality for managing zips
    /// </summary>
    static public class CommonZipFunctions
    {
        /// <summary>
        /// UnZip ZipFiles in output directory
        /// </summary>
        /// <param name="fileFullPathName"></param>
        /// <param name="outputDirectory"></param>
        static public void UnZip(string fileFullPathName, string outputDirectory)
        {
            ZipInputStream myZipInputStream = new ZipInputStream(File.OpenRead(fileFullPathName));
            ZipEntry myZipEntry;

            while ((myZipEntry = myZipInputStream.GetNextEntry()) != null)
            {
                if (myZipEntry.IsDirectory)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(outputDirectory, myZipEntry.Name)));
                }
                else
                {
                    // Ensure non-empty file name
                    if (myZipEntry.Name.Length > 0)
                    {
                        // Create a new file, get the file stream
                        FileStream myFileStream = File.Create(Path.Combine(outputDirectory, myZipEntry.Name));
                        int size = 2048;
                        byte[] data = new byte[2048];

                        // Write out the file
                        while (true)
                        {
                            size = myZipInputStream.Read(data, 0, data.Length);

                            if (size > 0)
                            {
                                myFileStream.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }

                        myFileStream.Close();
                    }
                }
            }

            myZipInputStream.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filesToZip"></param>
        /// <param name="path"></param>
        /// <param name="compression"></param>
        /// <param name="IgnoreFileNames">KeyValuePair name and mimeType</param>
        /// <param name="mimeType"></param>
        public static void WriteZipFileIgnoreCase(List<string> filesToZip, string path, int compression, List<KeyValuePair<string, string>> IgnoreFileNames)
        {
            ValidateCompressionRate(compression);

            ValidateDirectoryExist(path);

            ValidateFilesExiste(filesToZip);

            Crc32 crc32 = new Crc32();
            ZipOutputStream stream = new ZipOutputStream(File.Create(path));
            stream.SetLevel(compression);

            for (int i = 0; i < filesToZip.Count; i++)
            {
                string zipEntryName = Path.GetFileName(filesToZip[i]);
                string zipEntryNameNoMimeType = zipEntryName.Split('.')[0];

                //if (zipEntryNameNoMimeType.Equals(IgnoreFileName) == false)
                if (IgnoreFileNames.Exists(c => c.Key == zipEntryNameNoMimeType) == false)
                {
                    string fullPathNameFile = filesToZip[i];

                    AddFileToZipoutputStrem(crc32, stream, zipEntryName, fullPathNameFile);
                }
            }

            stream.Finish();
            stream.Close();
        }

        private static void AddFileToZipoutputStrem(Crc32 crc32, ZipOutputStream stream, string zipEntryName, string fullPathNameFile)
        {
            ZipEntry entry = new ZipEntry(zipEntryName);
            entry.DateTime = DateTime.Now;

            using (FileStream fs = File.OpenRead(fullPathNameFile))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                //entry.Size = fs.Length;
                fs.Close();
                crc32.Reset();
                crc32.Update(buffer);
                entry.Crc = crc32.Value;
                stream.PutNextEntry(entry);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filesToZip"></param>
        /// <param name="path"></param>
        /// <param name="compression"></param>
        public static void WriteZipFile(List<string> filesToZip, string path, int compression, string AddFileName, string mimeType, byte[] bytecontent)
        {
            ValidateCompressionRate(compression);

            ValidateDirectoryExist(path);

            ValidateFilesExiste(filesToZip);

            Crc32 crc32 = new Crc32();
            ZipOutputStream stream = new ZipOutputStream(File.Create(path));
            stream.SetLevel(compression);

            for (int i = 0; i < filesToZip.Count; i++)
            {
                string zipEntryName = Path.GetFileName(filesToZip[i]);
                string zipEntryNameNoMimeType = zipEntryName.Split('.')[0];

                if (zipEntryNameNoMimeType.Equals(AddFileName) == false)
                {
                    string fullPathNameFile = filesToZip[i];

                    AddFileToZipoutputStrem(crc32, stream, zipEntryName, fullPathNameFile);
                }
            }

            if (bytecontent != null)
                AddStreamFileToZipOutputStream(stream, new byte[bytecontent.Length], AddFileName + mimeType, bytecontent);

            stream.Finish();
            stream.Close();
        }

        private static void ValidateFilesExiste(List<string> filesToZip)
        {
            foreach (string c in filesToZip)
                if (!File.Exists(c))
                    throw new ArgumentException(string.Format("The File {0} does not exist!", c));
        }

        private static void ValidateDirectoryExist(string path)
        {
            if (!Directory.Exists(new FileInfo(path).Directory.ToString()))
                throw new ArgumentException("The Path does not exist.");
        }

        private static void ValidateCompressionRate(int compression)
        {
            if (compression < 0 || compression > 9)
                throw new ArgumentException("Invalid compression rate.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipStream"></param>
        /// <param name="buffer"></param>
        /// <param name="zipEntryName"></param>
        /// <param name="byteContent"></param>
        public static void AddStreamFileToZipOutputStream(ZipOutputStream zipStream, byte[] buffer, string zipEntryName, byte[] byteContent)
        {
            var fileName = ZipEntry.CleanName(zipEntryName);

            ZipEntry entry = new ZipEntry(fileName);
            zipStream.PutNextEntry(entry);
            var inStream = new MemoryStream(byteContent);
            StreamUtils.Copy(inStream, zipStream, buffer);
            inStream.Close();
        }
    }
   
    class ImportEventListener : DefaultEvents
    {
        public override bool OnError(DtsObject source, int errorCode, string subComponent, string description, string helpFile, int helpContext, string idofInterfaceWithError)
        {
            // Add application-specific diagnostics here.
            Console.WriteLine("Error in {0}/{1} : {2}", source, subComponent, description);
            return false;
        }
    }

    public static class MyExtensions
    {

        public static object ParseIntDBNull(this String str)
        {
            if (str == "")
                return DBNull.Value;

            return int.Parse(str);
        }

        public static object ParseISODateDBNull(this String str)
        {
            if (str == "")
                return DBNull.Value;

            return DateTimeLibrary.ISODateToDateTime(str);
        }

        public static object ParseDoubleDBNull(this String str)
        {
            if (str == "")
                return DBNull.Value;

            CultureInfo culture = CultureInfo.InvariantCulture;
            string strReturnValue = str.Replace(",", ".");

            return double.Parse(strReturnValue, culture);
        }

        public static double ParseDoubleZero(this String str)
        {
            if (str == "")
                return 0.0;

            CultureInfo culture = CultureInfo.InvariantCulture;
            string strReturnValue = str.Replace(",", ".");

            return double.Parse(strReturnValue, culture);
        }

        public static object ParseStringDBNull(this String str)
        {
            if (str == "")
                return DBNull.Value;

            return str;
        }
    }




    public class ReportServer2010Credentials : System.Net.NetworkCredential
    {
        private Cookie mAuthCookie;

        public ReportServer2010Credentials()
        {
            mAuthCookie = new System.Net.Cookie(FormsAuthentication.FormsCookieName, HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value, "/", CommonFunctions.GetConfigFileValue("WebService[@name='ReportingService']/Location/@domain"));
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {

            try
            {
                authCookie = mAuthCookie;
                user = password = authority = null;

                return true;  // Use forms credentials to authenticate.
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public System.Security.Principal.WindowsIdentity ImpersonationUser
        {

            get
            {
                try
                {
                    return null;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public System.Net.ICredentials NetworkCredentials
        {

            get
            {

                try
                {
                    return null;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


    }

    #endregion

    public static class ExcelStream
    {

        public static byte[] ExportExcelToByte(DataSet export, Array columnNames, Array columnHeaderNames, char seperator = ',')
        {
            ExcelPackage pck = null;
            OfficeOpenXml.ExcelWorkbook wb;
            ExcelWorksheet ws;
            byte[] result;
            int count = 2;
            int i = 0;

            try
            {
                // We start making the excel package
                pck = new ExcelPackage();

                // Translated from Tecncocom :
                // This excel template generates an error when it is loaded for the first time.
                // After searching on the web, the solution that I have found consists in loading the template a second time.
                // Besides altering/managing the library, I don't see any other solutions.
                try
                {
                    wb = pck.Workbook;
                }
                catch
                {
                    wb = pck.Workbook;
                }

                ws = wb.Worksheets.Add("List Results");

                for (i = 0; i < columnHeaderNames.Length; i++)
                {
                    ws.Cells[1, i + 1].Value = columnHeaderNames.GetValue(i);
                }

                // We set the data values
                foreach (DataRow dr in export.Tables[0].Rows)
                {
                    for (i = 1; i <= columnNames.Length; i++)
                    {
                        ws.Cells[count, i].Value = dr[columnNames.GetValue(i - 1).ToString()];
                    }

                    count++;
                }

                FormatCellsForDataType(ws, columnNames, export.Tables[0]);

                result = pck.GetAsByteArray();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                pck.Dispose();
            }
        }

        private static void FormatCellsForDataType(ExcelWorksheet ws, Array columnNames, DataTable dt)
        {
            ExcelRange er;

            int intDecimalsToDisplay = -1;

            //if(dt.Columns.Contains("DecimalsToDisplay") && dt.Rows.Count > 0)
            //{
            //    intDecimalsToDisplay = int.Parse(dt.Rows[0]["DecimalsToDisplay"].ToString());
            //}

            if (dt.Rows.Count > 0)
            {
                for (int i = 1; i <= columnNames.Length; i++)
                {
                    // Format all cells that contain Datetime date
                    if (dt.Columns[columnNames.GetValue(i - 1).ToString()].DataType == System.Type.GetType("System.DateTime"))
                    {
                        er = ws.Cells[2, i, dt.Rows.Count + 1, i];
                        er.Style.Numberformat.Format = @"dd-MM-yyyy HH:mm";
                    }

                    // Format cells with decimal values.
                    if (dt.Columns[columnNames.GetValue(i - 1).ToString()].DataType == System.Type.GetType("System.Decimal"))
                    {
                        if (dt.Columns.Contains("DecimalsToDisplay"))
                        {
                            int intRowIndex = 1;
                            foreach (DataRow dr in dt.Rows)
                            {
                                intDecimalsToDisplay = int.Parse(dr["DecimalsToDisplay"].ToString());

                                if (intDecimalsToDisplay == 0)
                                {
                                    er = ws.Cells[intRowIndex + 1, i];
                                    er.Style.Numberformat.Format = @"#,##0";
                                }
                                else if (intDecimalsToDisplay == 1)
                                {
                                    er = ws.Cells[intRowIndex + 1, i];
                                    er.Style.Numberformat.Format = @"#,##0.0";
                                }
                                else if (intDecimalsToDisplay == 2)
                                {
                                    er = ws.Cells[intRowIndex + 1, i];
                                    er.Style.Numberformat.Format = @"#,##0.00";
                                }
                                else if (intDecimalsToDisplay == 3)
                                {
                                    er = ws.Cells[intRowIndex + 1, i];
                                    er.Style.Numberformat.Format = @"#,##0.000";
                                }

                                intRowIndex++;
                            }
                            
                        }
                    }

                }
            }
        }
    }

    /// <summary>
    /// Static class that gathers all the enums and functions related to Security
    /// </summary>
    public static class SecurityCommon
    {

        public enum BusinessExceptions
        {

            AccessDeniedWithDetails = 90000,
            InvalidSecurityConfiguration = 90001,

            UserIncorrectOldPassword = 20000,
            UserIncorrectConfirmationPassword = 20001,
            UserLoginInvalidCredentials = 20002,
            UserLoginUserDiabled = 20003,
            UserLoginUserLocked = 20004,
            UserResetPasswordInvalidCredentials = 20005,
            UserResetPasswordUserDisabled = 20006,
            UserResetPasswordUserLocked = 20007,
            UserResetPasswordQuestionDoesNotExist = 20008,
            UserResetPasswordAnswerInvalid = 20009,
            UserResetPasswordIncorrectConfirmationPassword = 20010,
            UserResetPasswordSecurityQuestionsNoSet = 20011,
            UserSecurityQuestionsCannotChooseQuestionMoreThanOnce = 20012,
            UserResetPasswordQuestionsInvalidPassword = 20013,
            UserChangeResetPasswordInvalidAccountStatus = 20014,
            UserEmailAlreadyUsed = 200015,
            UserResetPasswordInvalidUserNameEmail = 20016,
            UserResetPasswordInvalidBirthDate = 20017,

        }

        public enum ClassID : long
        {
            Users,
            UserGroup,
            Groups,
            PickListValues
        }

        public enum AssociationID
        {
            Users_SecurityQuestions,
            Users_Groups,
            Users_History,
            Users_AccessRights,
            Users_UserEntityNames,
            Groups_Users,
            Groups_AccessRights,
            Groups_GroupEntityNames
        }

        public enum FormattedAssociationID
        {
            Users_GetGroups,
            Users_GetHistory,
            Users_GetAccessRights,
            Groups_GetUsers,
            Groups_GetAccessRights
        }

        public enum ReadComplexID
        {
            Users_ReadForEntityRelations,
            Groups_ReadForEntityRelations,
            Users_ReadComplex_GetGroups,
            Users_ReadComplex_ReadByUserName,
            Users_ReadComplex_ReadByEmail,
            Groups_ReadComplex_GetUsers,
            Security_ReadComplex_GetAccessRights,
            Users_ReadComplex_ReadRandomQuestion,
            Users_ReadComplex_GetCurrentUser,
            Users_ReadComplex_GetCurrentUserAccessRights,

            PickListValues_ReadComplex_GetPickListValues,
            PickListValues_ReadComplex_GetPickListValuesByParentCode,
            PickListValues_ReadComplex_GetMultiplePickListValues,
            PickListValues_ReadComplex_ReadPickListValues,
            Users_ReadComplex_CheckUsernameUnicity,
            Groups_ReadComplex_CheckGroupNameUnicity,
            Groups_ReadComplex_CheckUsernameUnicity,
            Users_ReadComplex_CheckGroupUnicity,
            Users_ReadComplex_GetUserAccessRights,
            Users_ReadComplex_GetUserSectionRoles,
        }

        public enum ExecuteComplexID
        {
            UserGroup_ExecuteComplex_AddRemoveUserGroups,
            Security_ExecuteComplex_SetAccessRights,
            Users_ExecuteComplex_ChangePassword,
            Users_ExecuteComplex_ChangeResetPassword,

            Users_ExecuteComplex_SetSecurityQuestions,
            Users_ExecuteComplex_IsSecurityQuestionsUpdateNeeded
        }

        /// <summary>
        /// An enumeration of the different call types for security purposes
        /// </summary>
        public enum CallID
        {
            Find,
            Create,
            Read,
            Insert,
            Update,
            Delete,
            Access,
            GetAssoc,
            ReadAssoc,
            CreateAssoc,
            InsertAssoc,
            UpdateAssoc,
            DeleteAssoc,
            ManageAssoc,
            ReadComplex,
            ExecuteComplex,
            Specific
        }

        public class CacheItemNames
        {
            public const string UserAccessRights = "useraccessrights";
            public const string CallSecurity = "CallSecurity";
            public const string UIConstants = "Azur.MyriadApp.Constants";
        }


        public static bool UserHasPermission(XmlDocument objXMLPermissions, string strResourceCode, string strPermissionCode)
        {

            XmlNode nodPermission = default(XmlNode);


            try
            {
                //// No codes, no access
                if (strResourceCode == null || string.IsNullOrEmpty(strResourceCode))
                    return false;
                if (strPermissionCode == null || string.IsNullOrEmpty(strPermissionCode))
                    return false;

                nodPermission = objXMLPermissions.SelectSingleNode("//Table[ResourceCode='" + strResourceCode + "']/" + strPermissionCode);

                if (nodPermission == null)
                {
                    return false;
                }

                if (nodPermission.InnerText == "1")
                {
                    return true;
                }

                return false;

            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// Clears the Cache for the current user
        /// </summary>
        public static void FlushCachedUserRights()
        {
            string strUserName = null;
            System.Web.HttpContext objContext = null;

            try
            {
                objContext = HttpContext.Current;
                strUserName = CommonFunctions.GetUserName();

                if ((objContext.Cache[CacheItemNames.UserAccessRights + strUserName] != null))
                {
                    objContext.Cache.Remove(CacheItemNames.UserAccessRights + strUserName);
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }


        public static XmlDocument GetXmlAccessRights()
        {
            //Gets the access rights from the server if they are not cached
            XmlDocument xmlAccessRights = new XmlDocument();
            string strXMLAccessRights = "";
            string strUserName = CommonFunctions.GetUserName();


            if ((HttpContext.Current.Cache[CacheItemNames.UserAccessRights + strUserName] != null))
            {
                return HttpContext.Current.Cache[CacheItemNames.UserAccessRights + strUserName] as XmlDocument;
            }
            else //No access rights found in the cache
            {
                WebRef.SecurityMngt.WebServiceClient objWebRef;
                EndpointAddress endPoint = new EndpointAddress(CommonFunctions.GetWSValue(azWebServices.SecurityService, azConfigFileWSInfo.URL));
                objWebRef = new WebRef.SecurityMngt.WebServiceClient(CommonFunctions.GetNewCustomBinding(false), endPoint);

                using (new OperationContextScope(objWebRef.InnerChannel))
                {
                    // Embeds the extracted cookie in the next web service request
                    HttpRequestMessageProperty request = new HttpRequestMessageProperty();
                    request.Headers["Cookie"] = HttpContext.Current.Request.Headers["Cookie"];
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = request;

                    //Temporarily pass the username 
                    strXMLAccessRights = objWebRef.ReadComplexBusinessObjects((long)SecurityCommon.ClassID.Users, (long)SecurityCommon.ReadComplexID.Users_ReadComplex_GetCurrentUserAccessRights,
                       CommonFunctions.GetUserName(), null);


                    xmlAccessRights.LoadXml(strXMLAccessRights);
                    HttpContext.Current.Cache.Insert(CacheItemNames.UserAccessRights + strUserName, xmlAccessRights, null, DateTime.MaxValue, TimeSpan.FromMinutes(5), System.Web.Caching.CacheItemPriority.Default, null);
                    return xmlAccessRights;
                }
            }
        }

        public static XmlDocument GetXmlAccessRightsByUsername(string strUserName)
        {
            //Gets the access rights from the server if they are not cached
            XmlDocument xmlAccessRights = new XmlDocument();
            string strXMLAccessRights = "";


            if ((HttpContext.Current.Cache[CacheItemNames.UserAccessRights + strUserName] != null))
            {
                return HttpContext.Current.Cache[CacheItemNames.UserAccessRights + strUserName] as XmlDocument;
            }
            else //No access rights found in the cache
            {
                WebRef.SecurityMngt.WebServiceClient objWebRef;
                EndpointAddress endPoint = new EndpointAddress(CommonFunctions.GetWSValue(azWebServices.SecurityService, azConfigFileWSInfo.URL));
                objWebRef = new WebRef.SecurityMngt.WebServiceClient(CommonFunctions.GetNewCustomBinding(false), endPoint);

                using (new OperationContextScope(objWebRef.InnerChannel))
                {
                    // Embeds the extracted cookie in the next web service request
                    HttpRequestMessageProperty request = new HttpRequestMessageProperty();
                    request.Headers["Cookie"] = HttpContext.Current.Request.Headers["Cookie"];
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = request;

                    //Temporarily pass the username 
                    strXMLAccessRights = objWebRef.ReadComplexBusinessObjects((long)SecurityCommon.ClassID.Users, (long)SecurityCommon.ReadComplexID.Users_ReadComplex_GetUserAccessRights,
                       strUserName/*CommonFunctions.GetUserName()*/, null);


                    xmlAccessRights.LoadXml(strXMLAccessRights);
                    HttpContext.Current.Cache.Insert(CacheItemNames.UserAccessRights + strUserName, xmlAccessRights, null, DateTime.MaxValue, TimeSpan.FromMinutes(5), System.Web.Caching.CacheItemPriority.Default, null);
                    return xmlAccessRights;
                }
            }
        }

        public static DataSet GetCurrentUser()
        {
            //Gets the access rights from the server if they are not cached
            XmlDocument xmlAccessRights = new XmlDocument();
            string strXMLCurrentUser = "";
            string strUserName = CommonFunctions.GetUserName();

            if (strUserName == "")
            {
                return null;
            }
            else
            {
                WebRef.SecurityMngt.WebServiceClient objWebRef;
                EndpointAddress endPoint = new EndpointAddress(CommonFunctions.GetWSValue(azWebServices.SecurityService, azConfigFileWSInfo.URL));

                objWebRef = new WebRef.SecurityMngt.WebServiceClient(CommonFunctions.GetNewCustomBinding(false), endPoint);

                using (new OperationContextScope(objWebRef.InnerChannel))
                {
                    // Embeds the extracted cookie in the next web service request
                    HttpRequestMessageProperty request = new HttpRequestMessageProperty();
                    request.Headers["Cookie"] = HttpContext.Current.Request.Headers["Cookie"];
                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = request;
                    //Temporarily pass the username 
                    strXMLCurrentUser = objWebRef.ReadComplexBusinessObjects((long)SecurityCommon.ClassID.Users, (long)SecurityCommon.ReadComplexID.Users_ReadComplex_GetCurrentUser, null, null);

                    DataSet dsCurrentUser = XMLLibrary.XML2DataSet(strXMLCurrentUser);

                    return dsCurrentUser;
                }
            }
        }

        public static DataSet GetUserByUsername(string strUserName)
        {
            //Gets the access rights from the server if they are not cached
            XmlDocument xmlAccessRights = new XmlDocument();
            string strXMLCurrentUser = "";

            WebRef.SecurityMngt.WebServiceClient objWebRef;
            EndpointAddress endPoint = new EndpointAddress(CommonFunctions.GetWSValue(azWebServices.SecurityService, azConfigFileWSInfo.URL));

            objWebRef = new WebRef.SecurityMngt.WebServiceClient(CommonFunctions.GetNewCustomBinding(false), endPoint);

            using (new OperationContextScope(objWebRef.InnerChannel))
            {
                // Embeds the extracted cookie in the next web service request
                HttpRequestMessageProperty request = new HttpRequestMessageProperty();
                request.Headers["Cookie"] = HttpContext.Current.Request.Headers["Cookie"];
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = request;

                Array arrParams = ArrayLibrary.CreateArray(strUserName);
                string strParams = ArrayLibrary.SystemArray2JSArray(ref arrParams);
                strXMLCurrentUser = objWebRef.ReadComplexBusinessObjects((long)SecurityCommon.ClassID.Users, (long)SecurityCommon.ReadComplexID.Users_ReadComplex_ReadByUserName,strParams /*null*/, null);

                DataSet dsCurrentUser = XMLLibrary.XML2DataSet(strXMLCurrentUser);

                return dsCurrentUser;
            }


        }

        public static void FlushWebServiceCachedUserRights()
        {

            WebRef.Services.WebServiceClient objWebRef;
            EndpointAddress endPoint = new EndpointAddress(CommonFunctions.GetWSValue(azWebServices.IATARSService, azConfigFileWSInfo.URL));
            objWebRef = new WebRef.Services.WebServiceClient(CommonFunctions.GetNewCustomBinding(false), endPoint);

            using (new OperationContextScope(objWebRef.InnerChannel))
            {
                // Embeds the extracted cookie in the next web service request
                HttpRequestMessageProperty request = new HttpRequestMessageProperty();
                request.Headers["Cookie"] = HttpContext.Current.Request.Headers["Cookie"];
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = request;

                objWebRef.FlushCachedUserRights();
            }

        }

        public static void FlushSecurityWebServiceCachedUserRights()
        {

            IATA.RS.Common.WebRef.SecurityMngt.WebServiceClient objWebRef;
            EndpointAddress endPoint = new EndpointAddress(CommonFunctions.GetWSValue(azWebServices.SecurityService, azConfigFileWSInfo.URL));
            objWebRef = new IATA.RS.Common.WebRef.SecurityMngt.WebServiceClient(CommonFunctions.GetNewCustomBinding(false), endPoint);

            using (new OperationContextScope(objWebRef.InnerChannel))
            {
                // Embeds the extracted cookie in the next web service request
                HttpRequestMessageProperty request = new HttpRequestMessageProperty();
                request.Headers["Cookie"] = HttpContext.Current.Request.Headers["Cookie"];
                OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = request;

                objWebRef.FlushCachedUserRights();
            }

        }
    }
}
