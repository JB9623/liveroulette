﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveRoulette.Criteria
{
    class MConsecutiveFS : ICriteria
    {
        private int sequence;
        private int SPINLIMIT = 11;
        private decimal paststake = 0;
        public MConsecutiveFS()
        {
            SetInitSequence();
        }
        public Global.BET_ITEMS CheckBetPlaces(List<Global.BET_SPOT> lstSpinAll, decimal startBettingAmount)
        {
            bool isFound = false;
            int repeat = 0;
            Global.BET_SPOT[] arrStats = GlobalData.GetStatsNumbersExceptZero(lstSpinAll);
            Global.BET_SPOT front = Global.GetBetHalf(arrStats[0]);
            Global.BET_ITEMS result = new Global.BET_ITEMS();

            SetInitSequence();
            /*if (lstSpinAll[0] == Global.BET_SPOT.ZERO)
                return result;*/

            for (repeat = 1; repeat < lstSpinAll.Count; repeat++)
            {
                if (Global.GetBetHalf(lstSpinAll[repeat]) == Global.GetBetHalf(lstSpinAll[repeat - 1]) && lstSpinAll[repeat] != Global.BET_SPOT.ZERO && lstSpinAll[repeat - 1] != Global.BET_SPOT.ZERO)
                    break;
            }

            repeat = repeat - 1;

            if (repeat < SPINLIMIT)
            {
                paststake = 0;
                SetInitSequence();
                return new Global.BET_ITEMS();
            }

            sequence = repeat - SPINLIMIT;

            decimal stake = 0;
            if (paststake == 0)
                stake = GlobalData.GetStartStake(startBettingAmount) * (decimal)Math.Pow(2, sequence);
            else
                stake = paststake * 2;

            stake = Math.Round(stake, 1);

            result.betStake = new decimal[] { stake };
            result.placeCount = 1;
            result.arrBetSpots = new Global.BET_SPOT[] { front };

            paststake = stake;

            return result;
        }

        public string GetCriteriaName()
        {
            //return "M Consecutive 1-18/19-36";
            return "Algorithm M";
        }
        public void SetInitSequence()
        {
            sequence = 0;
        }
    }
}
