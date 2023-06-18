import {Ecu} from './ecu';

export class Ticket {
    id: number = -1;
    
    customer_code: string = '';
    customer_level: string = '';
    
    user_name: string = '';
    date: Date = new Date();
    filename: string  = '';
    file_size: number  = 0;
    immatriculation: string = '';
    fuel: string = '';
   
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