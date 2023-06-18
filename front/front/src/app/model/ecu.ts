export class Ecu {

    constructor(data: any) {
        this.code = data.code;
        this.brand_code = data.Brand_code;
        this.fuel = data.fuel;
        this.dpf= data.dpf;
        this.egr= data.egr;
        this.lambda= data.lambda;
        this.hotstart= data.hotstart;
        this.flap= data.flap;
        this.adblue= data.adblue;
        this.dtc= data.dtc;
        this.torqmonitor= data.torqmonitor;
        this.speedlimit= data.speedlimit;
        this.startstop= data.startstop;
        this.nox= data.nox;
        this.tva= data.tva;
        this.readiness= data.readiness;
        this.immo= data.immo;
        this.maf= data.maf;
        this.hardcut= data.hardcut;
        this.displaycalibration= data.displaycalibration;
        this.waterpump= data.waterpump;
        this.tprot= data.tprot;
        this.o2= data.o2;
        this.glowplugs= data.glowplugs;
        this.y75= data.y75;
        this.special= data.special;
        this.decata= data.decata;
        this.vmax= data.vmax;
        this.stage1= data.stage1;
        this.stage2= data.stage2;
        this.flexfuel= data.flexfuel;

    }

    brand_code: string;
    code: string;
    fuel: string;

    dpf: boolean;
    egr: boolean;
    lambda: boolean;
    hotstart: boolean;
    flap: boolean;
    adblue: boolean;
    dtc: boolean;
    torqmonitor: boolean;
    speedlimit: boolean;
    startstop: boolean;
    nox: boolean;
    tva: boolean;
    readiness: boolean;
    immo: boolean;
    maf: boolean;
    hardcut: boolean;
    displaycalibration: boolean;
    waterpump: boolean;
    tprot: boolean;
    o2: boolean;
    glowplugs: boolean;
    y75: boolean;
    special: boolean;
    decata: boolean;
    vmax: boolean;
    stage1: boolean;
    stage2: boolean;
    flexfuel: boolean;
}