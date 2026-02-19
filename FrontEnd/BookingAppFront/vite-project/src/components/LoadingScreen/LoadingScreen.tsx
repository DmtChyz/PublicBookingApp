import styles from './LoadingScreen.module.css';

function LoadingScreen(){
    return(
        <div className={styles.loadingScreenBox}>
            <div className={styles.ldsRoller}>
                <div></div>
                <div></div>
                <div></div>
                <div></div>
                <div></div>
                <div></div>
                <div></div>
                <div></div>
            </div>
            <h2>Fetching some data..</h2>
        </div>
    )
}
export default LoadingScreen;