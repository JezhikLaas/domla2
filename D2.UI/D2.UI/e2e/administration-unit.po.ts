import { browser, by, element } from 'protractor';

export class AppPage {
  navigateTo() {
    return browser.get('/administrationUnits');
  }

  getParagraphText() {
    return element(by.id('myId')).getText();
  }
}
