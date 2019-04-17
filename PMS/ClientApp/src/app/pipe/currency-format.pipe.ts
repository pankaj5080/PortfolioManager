import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'currencyFormat'
})
export class CurrencyFormatPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    if (value) {
      var isLoss = value < 0;
      var valueString = isLoss ? value.toString().substring(1) : value.toString(); 
      var str = valueString.indexOf('.') > -1
        ? valueString.substring(0, valueString.indexOf('.')) : valueString;
      var prec = valueString.indexOf('.') > -1
        ? valueString.substring(valueString.indexOf('.')): '';
      var textAmount = "";

      if (str.length <= 3) {
        textAmount = str;
      }
      if (str.length > 3) {
        textAmount = [str.slice(0, str.length - 3), ",", str.slice(str.length - 3)].join('');
      }
      if (str.length > 5) {
        textAmount = [textAmount.slice(0, textAmount.length - 6), ",", textAmount.slice(textAmount.length - 6)].join('');
      }
      if (str.length > 7) {
        textAmount = [textAmount.slice(0, textAmount.length - 9), ",", textAmount.slice(textAmount.length - 9)].join('');
      }
      if (str.length > 9) {
        textAmount = [textAmount.slice(0, textAmount.length - 12), ",", textAmount.slice(textAmount.length - 12)].join('');
      }
      return isLoss ? '-' +  textAmount + prec : textAmount + prec;
    }
    return "0";
  }

}
