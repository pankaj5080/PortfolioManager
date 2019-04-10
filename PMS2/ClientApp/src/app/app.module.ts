import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap'
import { ChartsModule } from 'ng2-charts';
import { DatePickerModule } from '@syncfusion/ej2-angular-calendars';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { NavbarComponent } from './nav-bar/navbar.component';
import { NewPortfolioComponent } from './new-portfolio/new-portfolio.component';
import { PortfolioListComponent } from './portfolio-list/portfolio-list.component';
import { PortfolioCardComponent } from './portfolio-card/portfolio-card.component';
import { FundComponent } from './fund/fund.component';
import { ProfitAndLossComponent } from './profit-and-loss/profit-and-loss.component';
import { NewTradeComponent } from './new-trade/new-trade.component'
import { EditPortfolioComponent } from './edit-portfolio/edit-portfolio.component';
import { DashboardComponent } from './dashboard/dashboard.component'

import { PortfolioService } from './service/portfolio-service.service';
import { CurrencyFormatPipe } from './pipe/currency-format.pipe';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    NavbarComponent,
    NewPortfolioComponent,
    PortfolioListComponent,
    PortfolioCardComponent,
    FundComponent,
    ProfitAndLossComponent,
    NewTradeComponent,
    EditPortfolioComponent,
    CurrencyFormatPipe,
    DashboardComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    RouterModule.forRoot([
      { path: '', component: DashboardComponent, pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'new-portfolio', component: NewPortfolioComponent },
      { path: 'portfolio-list', component: PortfolioListComponent },
      { path: 'funds', component: FundComponent },
      { path: 'pnl', component: ProfitAndLossComponent },
      { path: 'edit-portfolio/:id', component: EditPortfolioComponent },
    ]),
    NgbModule.forRoot(),
    ChartsModule,
    DatePickerModule
  ],
  providers: [
    PortfolioService
  ],
  bootstrap: [AppComponent],
  entryComponents: [NewTradeComponent]

})
export class AppModule { }
