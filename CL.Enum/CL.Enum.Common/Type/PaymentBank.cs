using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Enum.Common.Type
{
    public enum PaymentBank
    {
        [Description("线下")]
        CAILE,

        [Description("财付通")]
        CFT,

        [Description("支付宝")]
        ZFB,

        [Description("中国人民银行信用卡")]
        PBC_CREDIT,

        [Description("中国人民银行借记卡")]
        PBC_DEBIT,
        
        [Description("国家开发银行信用卡")]
        CDB_CREDIT,

        [Description("国家开发银行借记卡")]
        CDB_DEBIT,
        
        [Description("工商银行信用卡")]
        ICBC_CREDIT,

        [Description("工商银行借记卡")]
        ICBC_DEBIT,
        
        [Description("农业银行信用卡")]
        ABC_CREDIT,

        [Description("农业银行借记卡")]
        ABC_DEBIT,
        
        [Description("建设银行信用卡")]
        CCB_CREDIT,

        [Description("建设银行借记卡")]
        CCB_DEBIT,
        
        [Description("交通银行信用卡")]
        COMM_CREDIT,

        [Description("交通银行借记卡")]
        COMM_DEBIT,
        
        [Description("中国银行信用卡")]
        BOC_CREDIT,

        [Description("中国银行借记卡")]
        BOC_DEBIT,
        
        [Description("招商银行信用卡")]
        CMB_CREDIT,

        [Description("招商银行借记卡")]
        CMB_DEBIT,
        
        [Description("民生银行信用卡")]
        CMBC_CREDIT,

        [Description("民生银行借记卡")]
        CMBC_DEBIT,
        
        [Description("兴业银行信用卡")]
        CIB_CREDIT,

        [Description("兴业银行借记卡")]
        CIB_DEBIT,
        
        [Description("光大银行信用卡")]
        CEB_CREDIT,

        [Description("光大银行借记卡")]
        CEB_DEBIT,
        
        [Description("中信银行信用卡")]
        CITIC_CREDIT,

        [Description("中信银行借记卡")]
        CITIC_DEBIT,
        
        [Description("华夏银行信用卡")]
        HXB_CREDIT,

        [Description("华夏银行借记卡")]
        HXB_DEBIT,
        
        [Description("北京银行信用卡")]
        BOB_CREDIT,

        [Description("北京银行借记卡")]
        BOB_DEBIT,
        
        [Description("广东发展银行信用卡")]
        GDB_CREDIT,

        [Description("广东发展银行借记卡")]
        GDB_DEBIT,
        
        [Description("浦东发展银行信用卡")]
        SPDB_CREDIT,

        [Description("浦东发展银行借记卡")]
        SPDB_DEBIT,
        
        [Description("深圳发展银行信用卡")]
        SDB_CREDIT,

        [Description("深圳发展银行借记卡")]
        SDB_DEBIT,
        
        [Description("汇丰银行信用卡")]
        HSBC_CREDIT,

        [Description("汇丰银行借记卡")]
        HSBC_DEBIT,
        
        [Description("恒丰银行信用卡")]
        EGB_CREDIT,

        [Description("恒丰银行借记卡")]
        EGB_DEBIT,

    }
}
