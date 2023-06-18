export class Config {

   //private static _appUrl = '/ps/api';
    private static _appUrl = ':5000';

    public  static DATE_TIME_FORMAT = 'DD-MM-YYYY HH:mm';
    public  static DATE_TIME_FORMAT_SECOND = 'DD-MM-YYYY HH:mm:ss';
    // timeout 5 min (5x60) for renewal
    public  static TIMEOUT_CHECK = 300000;
    public  static MAX_WAIT_REPORT = 600000;

    public  static STORAGE_PS_SIDEBAR_LARGE = 'ps-sidebar-large';
    public  static STORAGE_PS_SIDEBAR_ITEM = 'ps-sidebar-item';
    public  static STORAGE_USER_OBJ = 'ps-user';
    public  static STORAGE_LOGIN = 'ps-login';
    public  static STORAGE_TENANT = 'ps-tenant';
    public  static STORAGE_ACCESS_TOKEN = 'ps-access-token';

    public static CALENDAR_FR = {
        firstDayOfWeek: 1,
        dayNames: ['Dimanche', 'Lundi', 'Mardi', 'Mercredi', 'Jeudi', 'Vendredi', 'Samedi'],
        dayNamesShort: ['Dim', 'Lun', 'Mar', 'Mer', 'Jeu', 'Ven', 'Sam'],
        dayNamesMin: ['Di', 'Lu', 'Ma', 'Me', 'Je', 'Ve', 'Sa'],
        monthNames: ['Janvier', 'Février', 'Mars', 'Avril', 'Mai', 'Juin', 'Juillet', 'Août', 'Septembre', 'Octobre', 'Novembre', 'Décembre'],
        monthNamesShort: ['Jan', 'Fév', 'Mar', 'Avr', 'Mai', 'Jui', 'Jul', 'Aoû', 'Sep', 'Oct', 'Nov', 'Déc'],
        today: 'Aujourd\'hui',
        clear: 'Vider',
        dateFormat: 'dd/mm/yy',
        weekHeader: 'Semaine'
    };

    public static get APP_VERSION(): string {return '(c) Triviatech 2023 V1.0.0'; }
    public static get APP_TITLE(): string {return ' Patch Services Developement'; }
    public static get APP_URL(): string {return Config._appUrl; }

    public static INIT_APP_URL(url: string): void {
        Config._appUrl = url;
    }

    public static get API_ROUTES(): any {
        return {
            request_patch: '/request_patch',
            ping: '/auth/ping',
            brand: '/brand',
            ecu: '/ecu'
        };
    }
}