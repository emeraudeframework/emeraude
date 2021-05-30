import {loggerServiceAgent} from "../../api/endpoints";

export default {
    data() {
      return {

      }
    },
    computed: {
        languagePrefix() {
            return this.$store.getters.languageCode === 'en' ? "" : "/" + this.$store.getters.languageCode;
        }
    },
    methods: {
        getRoute(route = '') {
            return this.languagePrefix + route;
        },
        applyTranslationPropertiesToList(list) {
            for (let i = 0; i < list.length; i++) {
                list[i].translation = this.$t(list[i].nameKey);
            }

            return list;
        },
        redirectToRoute(route, clean = false) {
            window.location.href = this.getRoute(route, clean);
        },
        logError(error, caller) {
            loggerServiceAgent.logClientError({
                message: error.message,
                source: `${this.$options.name} (${window.location.href})`,
                method: caller,
                stackTrace: error.stack
            })
        }
    }
};