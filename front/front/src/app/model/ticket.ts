import {Ecu} from './ecu';

export class Ticket {

    constructor(data?: any) {
        if (data) {
            this.id = data.id;
            this.tenant = data.tenant;
            this.customer_level = data.customer_level;
            this.user_id = data.user_id;
            this.user_name = data.user_name;
            this.date = data.date;
            this.filename = data.filename;
            this.file_id = data.file_id;
            this.file_size = data.file_size;
            this.immatriculation = data.immatriculation;
            this.fuel = data.fuel;

            this.processed_filename = data.processed_filename;
            this.processed_file_size = data.processed_file_size;
            this.processed_user_name = data.processed_user_name;
            this.processed_user_id = data.processed_user_id;
            this.processed_date = data.processed_date;

            this.brand_code = data.brand_code;
            this.ecu_code = data.ecu_code;
            this.brand_name = data.brand_name;


            this.dpf = data.dpf;
            this.egr = data.egr;
            this.lambda = data.lambda;
            this.hotstart = data.hotstart;
            this.dtc = data.dtc;
            this.torqmonitor = data.torqmonitor;
            this.speedlimit = data.speedlimit;
            this.startstop = data.startstop;
            this.nox = data.nox;
            this.tva = data.tva;
            this.readiness = data.readiness;
            this.immo = data.immo;
            this.maf = data.maf;
            this.hardcut = data.hardcut;
            this.displaycalibration = data.displaycalibration;
            this.waterpump = data.waterpump;
            this.tprot = data.tprot;
            this.o2 = data.o2;

            this.glowplugs = data.glowplugs;
            this.y75 = data.y75;
            this.special = data.special;
            this.decata = data.decata;
            this.vmax = data.vmax;
            this.stage1 = data.stage1;
            this.stage2 = data.stage2;
            this.flexfuel = data.flexfuel;
        }
    }
    id: number = -1;
    
    // Processing info
    tenant: string = '';
    customer_level: string = '';
    user_id: number = 0;
    user_name: string = '';
    date: Date = new Date();
    filename: string  = '';
    file_size: number  = -1
    file_id: string|null  = null;
    immatriculation: string = '';
    fuel: string = '';

    // When ticket is processed
    processed_filename: string|null  = null;
    processed_file_size: number  = -1;
    processed_user_name: string|null  = null;
    processed_user_id: number = 0;
    processed_date: Date|null = null;
    
    // Parameters
    brand_code: string = '';
    ecu_code: string = '';
    brand_name: string = '';

    dpf: boolean = false;
    egr: boolean  = false;
    lambda: boolean = false;
    hotstart: boolean = false;
    flap: boolean = false;
    adblue: boolean = false;
    dtc: boolean = false;
    torqmonitor: boolean = false;
    speedlimit: boolean = false;
    startstop: boolean = false;
    nox: boolean = false;
    tva: boolean = false;
    readiness: boolean = false;
    immo: boolean = false;
    maf: boolean = false;
    hardcut: boolean = false;
    displaycalibration: boolean = false;
    waterpump: boolean = false;
    tprot: boolean = false;
    o2: boolean = false;
    glowplugs: boolean = false;
    y75: boolean = false;
    special: boolean = false;
    decata: boolean = false;
    vmax: boolean = false;
    stage1: boolean = false;
    stage2: boolean = false;
    flexfuel: boolean = false;

    // cache
    processing: string|null = null;

    updateFromEcu(ecu: Ecu) {
        this.dpf= ecu.dpf;
        this.egr= ecu.egr;
        this.lambda= ecu.lambda;
        this.hotstart= ecu.hotstart;
        this.flap= ecu.flap;
        this.adblue= ecu.adblue;
        this.dtc= ecu.dtc;
        this.torqmonitor= ecu.torqmonitor;
        this.speedlimit= ecu.speedlimit;
        this.startstop= ecu.startstop;
        this.nox= ecu.nox;
        this.tva= ecu.tva;
        this.readiness= ecu.readiness;
        this.immo= ecu.immo;
        this.maf= ecu.maf;
        this.hardcut= ecu.hardcut;
        this.displaycalibration= ecu.displaycalibration;
        this.waterpump= ecu.waterpump;
        this.tprot= ecu.tprot;
        this.o2= ecu.o2;
        this.glowplugs= ecu.glowplugs;
        this.y75= ecu.y75;
        this.special= ecu.special;
        this.decata= ecu.decata;
        this.vmax= ecu.vmax;
        this.stage1= ecu.stage1;
        this.stage2= ecu.stage2;
        this.flexfuel= ecu.flexfuel;
    }
}