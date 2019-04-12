import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { PortfolioType } from '../class/PortfolioType';
import { Portfolio } from '../class/Portfolio';
import { ProfitAndLoss } from '../class/ProfitAndLoss';
import { Trade } from '../class/Trade';
import { Dashboard } from '../class/Dashboard'
import { Networth } from '../class/Networth'

@Injectable()
export class PortfolioService {
  portfolioTypeApi = 'api/portfolioType';
  portfolioApi = 'api/portfolio';
  pnlApi = 'api/portfolio/getProfitDetails';
  tradeApi = 'api/trade';
  networthApi = 'api/networth';

  constructor(private http: HttpClient) { }

  getPortfolioTypes(): Observable<PortfolioType[]> {
    return this.http.get<PortfolioType[]>(this.portfolioTypeApi);
  }

  getPortfolioList(): Observable<Portfolio[]> {
    return this.http.get<Portfolio[]>(this.portfolioApi);
  }

  getPnlDetails(financialYear, profolioId): Observable<ProfitAndLoss> {
    return this.http.get<ProfitAndLoss>(this.pnlApi + "?financialYear=" + financialYear + "&portfolioId=" + profolioId);
  }

  createPortfolio(newPortfolio) {
    return this.http.post<Portfolio>(this.portfolioApi, newPortfolio);
  }

  addTrade(trade) {
    return this.http.post<Trade>(this.tradeApi, trade);
  }

  getPortfolioOpenTrades(portfolioId: number) {
    return this.http.get<Trade[]>(this.tradeApi + "/GetOpenPosition?portfolioId=" + portfolioId);
  }
  getPortfolioCompletedTrades(portfolioId) {
    return this.http.get<Trade[]>(this.tradeApi + "/GetPortfolioCompletedTrades?portfolioId=" + portfolioId);
  }
  getCompletedTrades() {
    return this.http.get<Trade[]>(this.tradeApi + "/GetCompletedTrades");
  }
  getPortfolioDetails(portfolioId) {
    return this.http.get<Portfolio>(this.portfolioApi + "/" + portfolioId);
  }
  getTrade(tradeId) {
    return this.http.get<Trade>(this.tradeApi + "/GetTrade?tradeId=" + tradeId);
  }
  getDashboardDetails() {
    return this.http.get<Dashboard>(this.tradeApi + "/GetDashboardDetails");
  }
  addNetworth(networth: Networth) {
    return this.http.post<Portfolio>(this.networthApi, networth);
  }
  getAllNetworth() {
    return this.http.get<Networth[]>(this.networthApi + "/GetAllNetwoth");
  }
  getLatestNetworth() {
    return this.http.get<Networth>(this.networthApi + "/GetLatestNetwoth");
  }
}
