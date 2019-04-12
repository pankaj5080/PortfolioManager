import { Trade } from "./Trade";
import { Networth } from "./Networth";

export class Dashboard {
  quarterProfit: number;
  yearProfit: number;
  overallProfit: number;
  tradingFunds: number;
  allTrades: Trade[];
  lastFiveTrades: Trade[];
  networths: Networth[];
}
