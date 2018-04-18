import {Entrance} from '../../../shared/entrance';

export class ErrorMessage {
  constructor(
    public forControl: string,
    public forValidator: string,
    public text: string
  ) { }
}

export const AdministrationUnitFormErrorMessages = [
  new ErrorMessage('Title', 'required', 'Ein Eingangstitel muss angegeben werden'),
  new ErrorMessage('UserKey', 'required', 'Es muss ein Benutzerschlüssel angegeben werden'),
  new ErrorMessage('Entrances', 'atLeastOneEntrance', 'Es muss mindestens ein Eingang mit einer vollständigen Addresse eingegeben werden'),

];

export const EntranceErrorMessages = [
  new ErrorMessage('Street', 'required', 'Geben Sie bitte eine Straße ein'),
  new ErrorMessage('City', 'required', 'Geben Sie bitte ein Ort ein'),
  new ErrorMessage('PostalCode', 'required', 'Geben Sie bitte eine PLZ ein'),
  new ErrorMessage('Number', 'required', 'Geben Sie bitte eine Hausnummer ein'),
];
