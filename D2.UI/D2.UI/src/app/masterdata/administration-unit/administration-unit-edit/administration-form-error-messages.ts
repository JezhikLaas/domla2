export class ErrorMessage {
  constructor(
    public forControl: string,
    public forValidator: string,
    public text: string
  ) { }
}

export const AdministrationUnitFormErrorMessages = [
  new ErrorMessage('Title', 'required', 'Es muss ein Objektname eingegeben werden'),
  new ErrorMessage('UserKey', 'required', 'Es muss ein Benutzerschlüssel eingegeben werden'),
  new ErrorMessage('Entrances', 'atLeastOneEntrance', 'Es muss mindestens ein Eingang mit einer vollständigen Addresse eingegeben werden'),
  new ErrorMessage('UserKey', 'maxlength', 'Es dürfen maximal 256 Zeichen eingegeben werden'),
  new ErrorMessage('Title', 'maxlength', 'Es dürfen maximal 256 Zeichen eingegeben werden'),

];

export const AddressErrorMessages = [
  new ErrorMessage('Street', 'required', 'Geben Sie bitte eine Straße ein'),
  new ErrorMessage('Street', 'maxLength', 'Es dürfen maximal 150 Zeichen eingegeben werden'),
  new ErrorMessage('City', 'required', 'Geben Sie bitte ein Ort ein'),
  new ErrorMessage('City', 'maxlength', 'Es dürfen maximal 100 Zeichen eingegeben werden'),
  new ErrorMessage('PostalCode', 'required', 'Geben Sie bitte eine PLZ ein'),
  new ErrorMessage('PostalCode', 'maxlength', 'Es dürfen maximal 20 Zeichen eingegeben werden'),
  new ErrorMessage('Number', 'required', 'Geben Sie bitte eine Hausnummer ein'),
  new ErrorMessage('Number', 'maxlength', 'Es dürfen maximal 10 Zeichen eingegeben werden'),
];

export const PropertiesErrorMessages = [
  new ErrorMessage('Title', 'required', 'Geben Sie bitte eine Zusatzfeld-Bezeichnung ein'),
  new ErrorMessage('Title', 'maxlength', 'Es dürfen maximal 256 Zeichen eingegeben werden'),
  new ErrorMessage('Description', 'maxlength', 'Es dürfen maximal 1024 Zeichen eingegeben werden'),
];

export const PropertyValueErrorMessages = [
  new ErrorMessage('Tag', 'required', 'Wählen Sie bitte den Typ des Zusatzfeldes aus'),
  new ErrorMessage('Raw', 'required', 'Geben Sie bitte den Wert des Zusatzfeldes ein'),
];

export const EntranceErrorMessages = [
  new ErrorMessage('Title', 'required', 'Es muss eine Bezeichnung eingegeben werden'),
  new ErrorMessage('Title', 'maxlength', 'Es dürfen maximal 256 Zeichen eingegeben werden'),
];

export const SubUnitsErrorMessages = [
  new ErrorMessage('Title', 'required', 'Es muss eine Bezeichnung eingegeben werden'),
  new ErrorMessage('Title', 'maxlength', 'Es dürfen maximal 256 Zeichen eingegeben werden'),
  new ErrorMessage('Number', 'required', 'Es muss eine Nummer eingegeben werden')
];
