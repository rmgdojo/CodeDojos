using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChainOfResponsibility
{
    public abstract class Actor
    {
        public virtual decimal TotalCostApprovalLimit { get; init; } = 0;
        public virtual decimal UnitCostApprovalLimit { get; init; } = 0;
        public virtual decimal QuantityApprovalLimit { get; init; } = 0;
        public Actor PreviousActor { get; set; }
        public Actor NextActor { get; init; }

        public virtual bool JoinsBoardMeetings => true;

        public virtual bool Approve(PurchaseOrder purchaseOrder, bool inBoardMeeting)
        {            
            return inBoardMeeting || (purchaseOrder.TotalCost <= TotalCostApprovalLimit 
                && purchaseOrder.UnitCost <= UnitCostApprovalLimit 
                && purchaseOrder.Quantity <= QuantityApprovalLimit);
        }

        public virtual (bool WasApproved, bool BoardMeetingWasRequired, IList<ApprovalResult> Approvals) SendForApproval(PurchaseOrder purchaseOrder)
        {
            bool approved = Approve(purchaseOrder);
            if (approved)
            {
                return (true, InBoardMeeting, new ApprovalResult[] { new ApprovalResult(true, this) });
            }
            else
            {
                if (NextActor != null)
                {
                    var result = NextActor.SendForApproval(purchaseOrder);
                    result.Approvals.Add(new ApprovalResult(false, this));
                }
                else
                {
                    // board meeting time
                    var previous = PreviousApprover;
                    while (true)
                    {
                        var nextPrevious = previous.PreviousApprover;
                        if (nextPrevious == null) break;
                    }

                    var result = PreviousApprover.
                    
                }
            }
        }

        public Actor(Actor nextActor)
        {
            NextActor = nextActor;
            NextActor.PreviousActor = this;
        }
    }

    public class Employee : Actor
    {
        public override bool JoinsBoardMeetings => false;
        
        public Employee(Actor nextActor) : base(nextActor) { }
    }

    public class FinanceEmployee : Actor
    {
        public override bool JoinsBoardMeetings => false;

        public FinanceEmployee(Actor nextActor) : base(nextActor) { }
    }


    public class Director : Actor
    {
        public override decimal TotalCostApprovalLimit => 5000;
        public override decimal UnitCostApprovalLimit => 1000;
        public override decimal QuantityApprovalLimit => 100;

        public Director(Actor nextActor) : base(nextActor) { }
    }

    public class CFO : Actor
    {
        public override decimal TotalCostApprovalLimit => 50000;
        public override decimal UnitCostApprovalLimit => 10000;
        public override decimal QuantityApprovalLimit => 1000;
        
        public CFO(Actor nextActor) : base(nextActor) { }

    }

    public class CEO : Actor
    {
        public override decimal TotalCostApprovalLimit => 500000;
        public override decimal UnitCostApprovalLimit => decimal.MaxValue;
        public override decimal QuantityApprovalLimit => decimal.MaxValue;

        public CEO(Actor nextActor) : base(nextActor) { }
    }
}
