using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NationApprovalRatings
{
    // Start is called before the first frame update


    //combined cannot be greater than 100
    public float positiveApproval = 0;
    public float negativeApproval = 0;
    public float neutralApproval = 100;

    public NationApprovalRatings() 
    {
        positiveApproval = 0;
        negativeApproval = 0;
        neutralApproval = 100;
    }


    public string NationApprovalRatingsToString()
    {
        string output = "";
        output += "positiveApproval: " + positiveApproval;
        output += "\nnegativeApproval: " + negativeApproval;
        output += "\nneutralApproval: " + neutralApproval;
        output += "\n";
        return output;
    }



    /**
     * Can only be increased by taking from neutral approval
     */
    public void IncreaseApproval(float approvalChange) 
    {
        if (approvalChange <= neutralApproval)
        {
            neutralApproval -= approvalChange;
            positiveApproval += approvalChange;

        }
        else 
        {
            positiveApproval += neutralApproval;
            neutralApproval = 0;
        }

    }
    public void DecreaseApproval(float approvalChange)
    {
        if (approvalChange <= neutralApproval)
        {
            neutralApproval -= approvalChange;
            negativeApproval += approvalChange;

        }
        else
        {
            negativeApproval += neutralApproval;
            neutralApproval = 0;
        }

    }


    /**
     * Takes away positive approval and adds to neutral
     */
    public void DecreasePositiveApproval(float approvalChange)
    {
    }

    /**
     * Takes away negative approval and adds to neutral
     */
    public void DecreaseNegativeApproval(float approvalChange)
    {
    }




}
