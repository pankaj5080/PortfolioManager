import { Trade } from "./Trade";

export class Dashboard {
  quarterProfit: number;
  yearProfit: number;
  overallProfit: number;
  tradingFunds: number;
  allTrades: Trade[];
  lastFiveTrades: Trade[];
}
