﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18010
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Walkabout.Ofx {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class OfxStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal OfxStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Walkabout.Ofx.OfxStrings", typeof(OfxStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified account number corresponds to an account that has been closed..
        /// </summary>
        internal static string AccountClosed {
            get {
                return ResourceManager.GetString("AccountClosed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user is not authorized to perform this action on the account, or the server does not allow this type of action to be performed on the account.
        /// </summary>
        internal static string AccountNotAuthorized {
            get {
                return ResourceManager.GetString("AccountNotAuthorized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified account number does not correspond to one of the user&apos;s accounts..
        /// </summary>
        internal static string AccountNotFound {
            get {
                return ResourceManager.GetString("AccountNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The transaction cannot be canceled or modified because it has already been canceled..
        /// </summary>
        internal static string AlreadyCanceled {
            get {
                return ResourceManager.GetString("AlreadyCanceled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The AUTHTOKEN sent by the client was invalid..
        /// </summary>
        internal static string AUTHTOKENInvalid {
            get {
                return ResourceManager.GetString("AUTHTOKENInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User needs to contact financial institution to obtain AUTHTOKEN. Client should send it in the next request..
        /// </summary>
        internal static string AUTHTOKENRequired {
            get {
                return ResourceManager.GetString("AUTHTOKENRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value of &lt;BANKNAME&gt; in the &lt;EXTBANKACCTTO&gt; aggregate is inconsistent with the value of &lt;BANKID&gt; in the &lt;BANKACCTTO&gt; aggregate..
        /// </summary>
        internal static string BankNameDoesntMatchBank {
            get {
                return ResourceManager.GetString("BankNameDoesntMatchBank", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A &lt;BRANCHID&gt; value must be provided in the &lt;BANKACCTFROM&gt; aggregate for this country system, but this field is missing.
        /// </summary>
        internal static string BranchIdMissing {
            get {
                return ResourceManager.GetString("BranchIdMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not support the &lt;CHGUSERINFORQ&gt; request..
        /// </summary>
        internal static string CannotChangeUserInformation {
            get {
                return ResourceManager.GetString("CannotChangeUserInformation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reserved for future use..
        /// </summary>
        internal static string CannotModifyDestinationAccount {
            get {
                return ResourceManager.GetString("CannotModifyDestinationAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not allow modifications to one or more values in a modification request..
        /// </summary>
        internal static string CannotModifyElement {
            get {
                return ResourceManager.GetString("CannotModifyElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reserved for future use..
        /// </summary>
        internal static string CannotModifySourceAccount {
            get {
                return ResourceManager.GetString("CannotModifySourceAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The CLIENTUID sent by the client was incorrect. User must register the Client UID..
        /// </summary>
        internal static string CLIENTUIDError {
            get {
                return ResourceManager.GetString("CLIENTUIDError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Based on the client timestamp, the client has the latest information. The response does not supply any additional information..
        /// </summary>
        internal static string ClientUptoDate {
            get {
                return ResourceManager.GetString("ClientUptoDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not support the &lt;PINCHRQ&gt; request..
        /// </summary>
        internal static string CouldNotChangeUSERPASS {
            get {
                return ResourceManager.GetString("CouldNotChangeUSERPASS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server could not generate random data as requested by the &lt;CHALLENGERQ&gt;..
        /// </summary>
        internal static string CouldNotProvideRandomData {
            get {
                return ResourceManager.GetString("CouldNotProvideRandomData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not support the country specified in the &lt;COUNTRY&gt; field of the &lt;SONRQ&gt; aggregate..
        /// </summary>
        internal static string CountrySystemNotSupported {
            get {
                return ResourceManager.GetString("CountrySystemNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server allows only one connection at a time, and another user is already signed on. Please try again later..
        /// </summary>
        internal static string CustomerAccountAlreadyInUse {
            get {
                return ResourceManager.GetString("CustomerAccountAlreadyInUse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server cannot accept requests for an action that far in the future.
        /// </summary>
        internal static string DateTooFarInFuture {
            get {
                return ResourceManager.GetString("DateTooFarInFuture", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server cannot process the requested action by the date specified by the user.
        /// </summary>
        internal static string DateTooSoon {
            get {
                return ResourceManager.GetString("DateTooSoon", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified account number corresponds to an account that has been closed.
        /// </summary>
        internal static string DestinationAccountClosed {
            get {
                return ResourceManager.GetString("DestinationAccountClosed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user is not authorized to perform this action on the account, or the server does not allow this type of action to be performed on the account.
        /// </summary>
        internal static string DestinationAccountNotAuthorized {
            get {
                return ResourceManager.GetString("DestinationAccountNotAuthorized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified account number does not correspond to one of the user&apos;s accounts.
        /// </summary>
        internal static string DestinationAccountNotFound {
            get {
                return ResourceManager.GetString("DestinationAccountNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A request with this &lt;TRNUID&gt; has already been received and processed.
        /// </summary>
        internal static string DuplicateRequest {
            get {
                return ResourceManager.GetString("DuplicateRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;REJECTIFMISSING&gt;Y and embedded transactions appeared in the request sync wrapper and the provided &lt;TOKEN&gt; was out of date. This code should be used in the &lt;SYNCERROR&gt; of the response sync wrapper..
        /// </summary>
        internal static string EmbeddedTransactionsInRequestFailed {
            get {
                return ResourceManager.GetString("EmbeddedTransactionsInRequestFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not support signons not accompanied by some other transaction..
        /// </summary>
        internal static string EmptySignonNotSupported {
            get {
                return ResourceManager.GetString("EmptySignonNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 401(k) information requested from a non-401(k) account..
        /// </summary>
        internal static string Error401kNotAvailableForThisAccount {
            get {
                return ResourceManager.GetString("Error401kNotAvailableForThisAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The FI requires the client to provide the &lt;FI&gt; aggregate in the &lt;SONRQ&gt; request, but either none was provided, or the one provided was invalid..
        /// </summary>
        internal static string FIMissingOrInvalidInSONRQ {
            get {
                return ResourceManager.GetString("FIMissingOrInvalidInSONRQ", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1098 forms are not yet available for the tax year requested..
        /// </summary>
        internal static string Form1098NotAvailable {
            get {
                return ResourceManager.GetString("Form1098NotAvailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user does not have any 1098 forms available..
        /// </summary>
        internal static string Form1098NotAvailableForUserID {
            get {
                return ResourceManager.GetString("Form1098NotAvailableForUserID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1099 forms are not yet available for the tax year requested..
        /// </summary>
        internal static string Form1099NotAvailable {
            get {
                return ResourceManager.GetString("Form1099NotAvailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This user does not have any 1099 forms available..
        /// </summary>
        internal static string Form1099NotAvailableForUserID {
            get {
                return ResourceManager.GetString("Form1099NotAvailableForUserID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Account error not specified by the remaining error codes.
        /// </summary>
        internal static string GeneralAccountError {
            get {
                return ResourceManager.GetString("GeneralAccountError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error other than those specified by the remaining error codes..
        /// </summary>
        internal static string GeneralError {
            get {
                return ResourceManager.GetString("GeneralError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not accept HTML formatting in the request..
        /// </summary>
        internal static string HTMLNotAllowed {
            get {
                return ResourceManager.GetString("HTMLNotAllowed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server cannot process the request because the specified account does not have enough funds..
        /// </summary>
        internal static string InsufficientFunds {
            get {
                return ResourceManager.GetString("InsufficientFunds", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid account.
        /// </summary>
        internal static string InvalidAccount {
            get {
                return ResourceManager.GetString("InvalidAccount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified amount is not valid for this action; for example, the user specified a negative payment amount.
        /// </summary>
        internal static string InvalidAmount {
            get {
                return ResourceManager.GetString("InvalidAmount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Response for non-overlapping dates, date ranges in the future, et cetera.
        /// </summary>
        internal static string InvalidateDateRange {
            get {
                return ResourceManager.GetString("InvalidateDateRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified datetime stamp cannot be parsed; for instance, the datetime stamp specifies 25:00 hours.
        /// </summary>
        internal static string InvalidDate {
            get {
                return ResourceManager.GetString("InvalidDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified frequency &lt;FREQ&gt; does not match one of the accepted frequencies for recurring transactions..
        /// </summary>
        internal static string InvalidFrequency {
            get {
                return ResourceManager.GetString("InvalidFrequency", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Payee error not specified by the remaining error codes.
        /// </summary>
        internal static string InvalidPayee {
            get {
                return ResourceManager.GetString("InvalidPayee", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The account number &lt;PAYACCT&gt; of the requested payee is invalid.
        /// </summary>
        internal static string InvalidPayeeAccountNumber {
            get {
                return ResourceManager.GetString("InvalidPayeeAccountNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Some portion of the payee&apos;s address is incorrect or unknown.
        /// </summary>
        internal static string InvalidPayeeAddress {
            get {
                return ResourceManager.GetString("InvalidPayeeAddress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified city is incorrect or unknown..
        /// </summary>
        internal static string InvalidPayeeCity {
            get {
                return ResourceManager.GetString("InvalidPayeeCity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified payee ID does not exist or no longer exists..
        /// </summary>
        internal static string InvalidPayeeID {
            get {
                return ResourceManager.GetString("InvalidPayeeID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified payee list ID does not exist or no longer exists..
        /// </summary>
        internal static string InvalidPayeeListID {
            get {
                return ResourceManager.GetString("InvalidPayeeListID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not recognize the specified payee name..
        /// </summary>
        internal static string InvalidPayeeName {
            get {
                return ResourceManager.GetString("InvalidPayeeName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified postal code is incorrect or unknown..
        /// </summary>
        internal static string InvalidPayeePostalCode {
            get {
                return ResourceManager.GetString("InvalidPayeePostalCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified state is incorrect or unknown..
        /// </summary>
        internal static string InvalidPayeeState {
            get {
                return ResourceManager.GetString("InvalidPayeeState", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not support the service &lt;SVC&gt; specified in the service-activation request..
        /// </summary>
        internal static string InvalidService {
            get {
                return ResourceManager.GetString("InvalidService", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server was unable to validate the TAN sent in the request.
        /// </summary>
        internal static string InvalidTan {
            get {
                return ResourceManager.GetString("InvalidTan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server could not parse the URL..
        /// </summary>
        internal static string InvalidURL {
            get {
                return ResourceManager.GetString("InvalidURL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not support investment balances download..
        /// </summary>
        internal static string InvestmentBalancesDownloadNotSupported {
            get {
                return ResourceManager.GetString("InvestmentBalancesDownloadNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not support open order download..
        /// </summary>
        internal static string InvestmentOpenOrderDownloadNotSupported {
            get {
                return ResourceManager.GetString("InvestmentOpenOrderDownloadNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not support investment position download..
        /// </summary>
        internal static string InvestmentPositionDownloadNotSupported {
            get {
                return ResourceManager.GetString("InvestmentPositionDownloadNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not support investment positions for the specified date..
        /// </summary>
        internal static string InvestmentPositionsForSpecifiedDateNotAvailable {
            get {
                return ResourceManager.GetString("InvestmentPositionsForSpecifiedDateNotAvailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not support investment transaction download..
        /// </summary>
        internal static string InvestmentTransactionDownloadNotSupported {
            get {
                return ResourceManager.GetString("InvestmentTransactionDownloadNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User credentials are correct, but further authentication required. Client should send &lt;MFACHALLENGERQ&gt; in next request..
        /// </summary>
        internal static string MFAChallengeAuthenticationRequired {
            get {
                return ResourceManager.GetString("MFAChallengeAuthenticationRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User or client information sent in MFACHALLENGEA contains invalid information.
        /// </summary>
        internal static string MFAChallengeInformationIsInvalid {
            get {
                return ResourceManager.GetString("MFAChallengeInformationIsInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User should contact financial institution..
        /// </summary>
        internal static string MFAError {
            get {
                return ResourceManager.GetString("MFAError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server has already canceled the specified recurring model..
        /// </summary>
        internal static string ModelAlreadyCanceled {
            get {
                return ResourceManager.GetString("ModelAlreadyCanceled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user must change his or her &lt;USERPASS&gt; number as part of the next OFX request..
        /// </summary>
        internal static string MustChangeUSERPASS {
            get {
                return ResourceManager.GetString("MustChangeUSERPASS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server could not find the requested securities..
        /// </summary>
        internal static string OneOrMoreSecuritiesNotFound {
            get {
                return ResourceManager.GetString("OneOrMoreSecuritiesNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not allow clients to change payee information..
        /// </summary>
        internal static string PayeeNotModifiableByClient {
            get {
                return ResourceManager.GetString("PayeeNotModifiableByClient", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This error code may appear in the &lt;SYNCERROR&gt; element of an &lt;xxxSYNCRS&gt; wrapper (in &lt;PRESDLVMSGSRSV1&gt; and V2 message set responses) or the &lt;CODE&gt; contained in any embedded transaction wrappers within a sync response. The corresponding sync request wrapper included &lt;REJECTIFMISSING&gt;Y with &lt;REFRESH&gt;Y or &lt;TOKENONLY&gt;Y, which is illegal.
        /// </summary>
        internal static string RejectIfMissingInvalidWithoutToken {
            get {
                return ResourceManager.GetString("RejectIfMissingInvalidWithoutToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to One or more elements of the request were not recognized by the server or the server (as noted in the FI Profile) does not support the elements. The server executed the element transactions it understood and supported. For example, the request file included private tags in a &lt;PMTRQ&gt; but the server was able to execute the rest of the request..
        /// </summary>
        internal static string RequestedElementUnknown {
            get {
                return ResourceManager.GetString("RequestedElementUnknown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user cannot signon because he or she entered an invalid user ID or password..
        /// </summary>
        internal static string SignonInvalid {
            get {
                return ResourceManager.GetString("SignonInvalid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The OFX block associated with the signon does not contain a pin change request and should..
        /// </summary>
        internal static string SignonInvalidWithoutSupportingPinChangeRequest {
            get {
                return ResourceManager.GetString("SignonInvalidWithoutSupportingPinChangeRequest", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified account number corresponds to an account that has been closed.
        /// </summary>
        internal static string SourceAccountClosed {
            get {
                return ResourceManager.GetString("SourceAccountClosed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user is not authorized to perform this action on the account, or the server does not allow this type of action to be performed on the account.
        /// </summary>
        internal static string SourceAccountNnotAuthorized {
            get {
                return ResourceManager.GetString("SourceAccountNnotAuthorized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified account number does not correspond to one of the user&apos;s accounts..
        /// </summary>
        internal static string SourceAccountNotFound {
            get {
                return ResourceManager.GetString("SourceAccountNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stop check is already in process.
        /// </summary>
        internal static string StopCheckInProcess {
            get {
                return ResourceManager.GetString("StopCheckInProcess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified table type is not recognized or does not exist..
        /// </summary>
        internal static string TableTypeNotFound {
            get {
                return ResourceManager.GetString("TableTypeNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The stop-payment request &lt;STPCHKRQ&gt; specifies too many checks.
        /// </summary>
        internal static string TooManyChecksToProcess {
            get {
                return ResourceManager.GetString("TooManyChecksToProcess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Transaction has entered the processing loop and cannot be modified/cancelled using OFX. The transaction may still be cancelled or modified using other means (for example, a phone call to Customer Service)..
        /// </summary>
        internal static string TransactionAlreadyCommitted {
            get {
                return ResourceManager.GetString("TransactionAlreadyCommitted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Transaction has already been sent or date due is past.
        /// </summary>
        internal static string TransactionAlreadyProcessed {
            get {
                return ResourceManager.GetString("TransactionAlreadyProcessed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Current user is not authorized to perform this action on behalf of the &lt;USERID&gt;..
        /// </summary>
        internal static string TransactionNotAuthorized {
            get {
                return ResourceManager.GetString("TransactionNotAuthorized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server could not enroll the user..
        /// </summary>
        internal static string UnableToEnrollUser {
            get {
                return ResourceManager.GetString("UnableToEnrollUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server was unable to retrieve the information at this URL (e.g., an HTTP 400 or 500 series error)..
        /// </summary>
        internal static string UnableToGetURL {
            get {
                return ResourceManager.GetString("UnableToGetURL", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Used in response transaction wrapper for embedded transactions when &lt;SYNCERROR&gt;6501 appears in the surrounding sync wrapper..
        /// </summary>
        internal static string UnableToProcessEmbeddedTransactionDueToOutOfDate {
            get {
                return ResourceManager.GetString("UnableToProcessEmbeddedTransactionDueToOutOfDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified FITID/BILLID does not exist or no longer exists.
        /// </summary>
        internal static string UnknownFITID {
            get {
                return ResourceManager.GetString("UnknownFITID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server was unable to send mail to the specified Internet address..
        /// </summary>
        internal static string UnknownMailTo {
            get {
                return ResourceManager.GetString("UnknownMailTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified model ID does not exist or no longer exists..
        /// </summary>
        internal static string UnknownModelID {
            get {
                return ResourceManager.GetString("UnknownModelID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified server ID does not exist or no longer exists.
        /// </summary>
        internal static string UnknownServerId {
            get {
                return ResourceManager.GetString("UnknownServerId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server does not support the requested version. The version of the message set specified by the client is not supported by this server..
        /// </summary>
        internal static string UnsupportedVersion {
            get {
                return ResourceManager.GetString("UnsupportedVersion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server has already enrolled the user..
        /// </summary>
        internal static string UserAlreadyEnrolled {
            get {
                return ResourceManager.GetString("UserAlreadyEnrolled", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server will send the user ID and password via postal mail, e-mail, or another means. The accompanying message will provide details..
        /// </summary>
        internal static string UserIDAndPasswordWillBeSentOutOfBand {
            get {
                return ResourceManager.GetString("UserIDAndPasswordWillBeSentOutOfBand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The server has received too many failed signon attempts for this user. Please call the FI&apos;s technical support number..
        /// </summary>
        internal static string UserPasslockout {
            get {
                return ResourceManager.GetString("UserPasslockout", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to W2 forms are not yet available for the tax year requested..
        /// </summary>
        internal static string W2formsNotAvailable {
            get {
                return ResourceManager.GetString("W2formsNotAvailable", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The user does not have any W2 forms available..
        /// </summary>
        internal static string W2FormsNotAvailableForUserID {
            get {
                return ResourceManager.GetString("W2FormsNotAvailableForUserID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The specified wire beneficiary does not exist or no longer exists..
        /// </summary>
        internal static string WireBeneficiaryInvalid {
            get {
                return ResourceManager.GetString("WireBeneficiaryInvalid", resourceCulture);
            }
        }
    }
}
