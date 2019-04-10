import { Trade } from "./Trade";

export class Portfolio {
  portfolioId: number;
  name: string;
  portfolioTypeId?: number;
  Type?: string;
  TypeId?: string;
  InitialAmount?: number;
  Positions?: number;
  positionValue?: number;
  profit?: number;
  NumberOfTrades?: number;
  accountValue?: number;
  percentageGain?: number;
  openPosition?: Trade[];
  completedTrades?: Trade[];
}
