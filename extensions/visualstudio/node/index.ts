import { extractMinimalStackTraceInfo } from '../../../core/util/extractMinimalStackTraceInfo.ts';

const info = extractMinimalStackTraceInfo(new Error('test').stack);
console.log(`Core util loaded: ${info}`);
