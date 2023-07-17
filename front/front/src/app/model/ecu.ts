export class Ecu {

    constructor(data: any) {
        this.code = data.code;
        this.brand_code = data.brand_code;
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

    processing: string = '';

    
  getProcessing(): string {
    if (this.processing != '') {
      return this.processing;
    }
    let result: string[] = [];
    if (this.dpf) { result.push('dpf');};
    if (this.egr) { result.push('egr');};
    if (this.lambda) { result.push('lambda');};
    if (this.hotstart) { result.push('hotstart');};
    if (this.flap) { result.push('flap');};
    if (this.adblue) { result.push('adblue');};
    if (this.dtc) { result.push('dtc');};
    if (this.torqmonitor) { result.push('torqmonitor');};
    if (this.speedlimit) { result.push('speedlimit');};
    if (this.startstop) { result.push('startstop');};
    if (this.nox) { result.push('nox');};
    if (this.tva) { result.push('tva');};
    if (this.readiness) { result.push('readiness');};
    if (this.immo) { result.push('immo');};
    if (this.maf) { result.push('maf');};
    if (this.hardcut) { result.push('hardcut');};
    if (this.displaycalibration) { result.push('displaycalibration');};
    if (this.waterpump) { result.push('waterpump');};
    if (this.tprot) { result.push('tprot');};
    if (this.o2) { result.push('o2');};
    if (this.glowplugs) { result.push('glowplugs');};
    if (this.y75) { result.push('y75');};
    if (this.special) { result.push('special');};
    if (this.decata) { result.push('decata');};
    if (this.vmax) { result.push('vmax');};
    if (this.stage1) { result.push('stage1');};
    if (this.stage2) { result.push('stage2');};
    if (this.flexfuel) { result.push('flexfuel');};

    let s = '';
    for(const p of result) {
      if (s.length > 0) {
        s = s + ', ';
      }
      s = s + p;
    }
    this.processing = s;
    return this.processing;
  }
}