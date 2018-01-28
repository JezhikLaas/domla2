export class ErrorMessage {
  constructor(
    public forControl: string,
    public forValidator: string,
    public message: string
  ) {}
}

export const ErrorMessages = [
  new ErrorMessage(
    'salutation',
    'required',
    'Geben Sie bitte eine Anrede an.'),
  new ErrorMessage(
    'lastname',
    'required',
    'Geben Sie bitte einen Namen an.'),
  new ErrorMessage(
    'lastname',
    'minlength',
    'Mindestens drei Buchstaben werden erwartet.'),
  new ErrorMessage(
    'username',
    'required',
    'Geben Sie bitte den gewünschten Nutzernamen an.'),
  new ErrorMessage(
    'username',
    'minlength',
    'Mindestens 8 Zeichen werden erwartet.'),
  new ErrorMessage(
    'email',
    'required',
    'Geben Sie bitte eine Email Adresse an.'),
  new ErrorMessage(
    'email',
    'email',
    'Die eingegebene Adresse ist nicht gültig.')
];
